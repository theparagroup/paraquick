using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.paralib.paraquick.Models.Ef;
using com.paralib.paraquick.qbxml;

namespace com.paralib.paraquick.qbwc
{
    public abstract class QbWcService : QbWcServiceBase
    {
        protected void Error(string message)
        {
            Logger.Error(message);
        }

        protected void Error(DbContext db, EfParaquickSession efSession, string message)
        {
            Error(message);
            ServiceUtils.SessionError(db, efSession, message);
        }

        protected override string OnAuthenticate(string username, string password, out AuthCodes authCode, out AuthOptions authOptions)
        {

            using (var db=ServiceUtils.CreateDbContext())
            {
                //Find the company based on the unique user name
                EfParaquickCompany efCompany = (from c in db.ParaquickCompanies where c.UserName == username && c.Password == password select c).FirstOrDefault();

                if (efCompany != null)
                {
                    authOptions = new AuthOptions();

                    EfParaquickSession efSession = ServiceUtils.FindNextSession(db, efCompany.Id);

                    //work to do?
                    if (efSession != null)
                    {
                        //allow implementors to override
                        authCode= OnNewSession(db, efSession, authOptions);

                        if (authCode==AuthCodes.VALID)
                        {
                            //open and return ticket
                            ServiceUtils.Open(db,efSession);
                            return efSession.Ticket;
                        }
                        else
                        {
                            //implementors can override the code and the options
                            //but not the session record or the ticket
                            return ServiceUtils.ZeroTicket;
                        }

                    }
                    else
                    {
                        //nothing to do
                        authCode = AuthCodes.NONE;
                    }

                }
                else
                {
                    //bad credentials
                    Error($"Incorrect username or password for user ({username})");
                    authCode = AuthCodes.NVU;
                }


                authOptions = null;
                return ServiceUtils.ZeroTicket;
            }
        }

        protected virtual AuthCodes OnNewSession(DbContext db, EfParaquickSession efSession, AuthOptions authOptions)
        {
            //the default behavior is to require the company file to be open ("") with no other options specified
            //but implmentations may want to do something different
            return AuthCodes.VALID;
        }


        protected override string OnConnectionError(string ticket, HResult hResult)
        {
            if (ticket != ServiceUtils.ZeroTicket)
            {
                using (var db = ServiceUtils.CreateDbContext())
                {
                    EfParaquickSession efSession = ServiceUtils.FindSession(db, ticket);

                    if (efSession != null)
                    {
                        //log this error
                        Error(db, efSession, hResult.Format());

                        //ask the implementation if we should tell the WC to retry
                        string companyFilePath;
                        if (OnRetryConnection(db, efSession, hResult, out companyFilePath))
                        {
                            return companyFilePath;
                        }
                        else
                        {
                            //if not, we reset the session to "New" and let the user fix it and try again
                            ServiceUtils.Reset(db, efSession);
                        }

                    }
                    else
                    {
                        //note: getLastError will not be called so we log this here
                        Error("Can't find ticket ({ticket}) during connection error");
                    }

                }
            }
            else
            {
                //note: getLastError will not be called so we log this here
                Error("Can't process zeroticket during connection error");
            }

            return "done";

        }

        protected virtual bool OnRetryConnection(DbContext db, EfParaquickSession efSession, HResult hResult, out string companyFilePath)
        {
            //default behavior is to not retry
            //but implmentations may want to instruct the webconnector to try again, optionally with another file
            companyFilePath = null;
            return false;
        }

        protected override string OnCreateRequestMessage(string ticket, string hcpXml, string companyFilePath, string qbCountry, int qbMajorVersion, int qbMinorVersion)
        {
            //zerotickets should never be seen here, but if so, they will recur in getLastError and be logged there
            if (ticket!=ServiceUtils.ZeroTicket)
            {
                using (var db = ServiceUtils.CreateDbContext())
                {
                    EfParaquickSession efSession = ServiceUtils.FindSession(db, ticket);

                    //bad tickets will recur in getLastError and be logged there
                    if (efSession != null)
                    {
                        //confirm file path
                        if (IsCompanyFileValid(db, efSession, companyFilePath))
                        {
                            //save the hcpXml & other information in the company record
                            if (!string.IsNullOrEmpty(hcpXml))
                            {
                                ServiceUtils.Session(db, efSession, hcpXml, qbCountry, qbMajorVersion, qbMinorVersion);
                            }

                            //find next message for this ticket
                            List<EfParaquickMessage> efMessages = ServiceUtils.FindNextMessageSet(db, efSession);

                            if (efMessages!=null)
                            {
                                //TODO do we set OnError to stopOnErrors in the request?
                                RqMsgSet rqMsgSet = new RqMsgSet();

                                foreach (var efMessage in efMessages)
                                {
                                    //deserialize request message
                                    IRqMsg rqMsg = (IRqMsg)Msg.Deserialize(efMessage.MessageType.RequestTypeName, efMessage.RequestXml);

                                    //allow implementation to see/modify message
                                    string errorMessage = OnRequest(db, efMessage, rqMsg);
                                    if (errorMessage == null)
                                    {
                                        //add it to message set
                                        rqMsgSet.Add(rqMsg);
                                    }
                                    else
                                    {
                                        //"close" this request, don't send to WC
                                        ServiceUtils.RequestError(db, efMessage, errorMessage);
                                    }

                                }

                                //TODO is "nothing to do" at this point an error?

                                //send it
                                string xml = rqMsgSet.Serialize();
                                return xml;
                            }
                            else
                            {
                                //something wrong with the message set in the database, log it
                                Error(db, efSession, $"An error occurred when building the message set for session ({efSession.Id})");

                                //this is pretty bad, let's just close the session
                                ServiceUtils.Close(db, efSession);
                            }

                        }
                        else
                        {
                            //log error
                            Error(db, efSession, $"Incorrect company file ({companyFilePath}) for company ({efSession.CompanyId})");
                        }
                    }

                }

            }

            //error condition
            return "";
        }

        protected virtual bool IsCompanyFileValid(DbContext db, EfParaquickSession efSession, string companyFilePath)
        {
            //the default behavior is to always use the file that is currently open,
            //but implmentations may want to do something different
            return true;
        }

        protected virtual string OnRequest(DbContext db, EfParaquickMessage efMessage, IRqMsg rqMsg)
        {
            //nothing wrong, proceed
            return null;
        }

        protected override int OnResponseMessage(string ticket, string responseXml, HResult hResult)
        {

            //zerotickets should never be seen here, but if so, they will recur in getLastError and be logged there
            if (ticket != ServiceUtils.ZeroTicket)
            {
                using (var db = ServiceUtils.CreateDbContext())
                {
                    EfParaquickSession efSession = ServiceUtils.FindSession(db, ticket);

                    //bad tickets will recur in getLastError and be logged there
                    if (efSession != null)
                    {
                        //deserialize response and process success/error
                        RsMsgSet rsMsgSet = new RsMsgSet();
                        QBXML qbxml=rsMsgSet.Deserialize(responseXml);

                        //TODO update paraquick entities based on response type
                        foreach (var rsMsg in rsMsgSet)
                        {
                            var efMessage = efSession.ParaquickMessages.Where(m => m.RequestId == rsMsg.requestID).FirstOrDefault();

                            if (efMessage != null)
                            {
                                ServiceUtils.Response(db, efMessage, rsMsg);

                                //allow implementor to do something with response
                                OnResponse(db, efMessage, rsMsg);

                                if (rsMsg.statusCode != "0")
                                {
                                    //TODO stop on errors?
                                }

                            }
                            else
                            {
                                //TODO stop on errors?
                                Error(db, efSession, $"Can't find request ({rsMsg.requestID})");
                            }

                        }


                        //TODO StopOnErrors? do we stop on errors here (return -1) or keep going?
                        //report "%" - completed messages/total messages for session
                        int pctComplete= ServiceUtils.CalculatePercentComplete(db, efSession);
                        return pctComplete;
                    }

                }
            }

            //error condition
            return -1;
        }

        protected virtual void OnResponse(DbContext db, EfParaquickMessage efMessage, IRsMsg rsMsg)
        {

        }

        protected override string OnGetLastError(string ticket)
        {
            string message;

            if (ticket == ServiceUtils.ZeroTicket)
            {
                //note: we should never see the zero tickets, but we report it here
                message = "Can't process zero ticket";
                Error(message);
            }
            else
            {
                using (var db = ServiceUtils.CreateDbContext())
                {
                    EfParaquickSession efSession = ServiceUtils.FindSession(db, ticket);

                    if (efSession != null)
                    {
                        //return response-level and ticket-level errors 
                        //(no need to log them again)
                        message = OnGetLastError(db, efSession);
                    }
                    else
                    {
                        //we report bad tickets here
                        message = $"can't find ticket ({ticket})";
                        Error(message);
                    }
                }
            }


            return message;
        }

        protected virtual string OnGetLastError(DbContext db, EfParaquickSession efSession)
        {
            return ServiceUtils.FormatErrors(efSession);
        }


        protected override string OnCloseConnection(string ticket)
        {
            //note: we should never see the zero ticket here either
            //      but it's already been logged at this point
            if (ticket != ServiceUtils.ZeroTicket)
            {
                using (var db = ServiceUtils.CreateDbContext())
                {
                    EfParaquickSession efSession = ServiceUtils.FindSession(db, ticket);

                    //bad tickets have already been logged
                    if (efSession != null)
                    {
                        //close session
                        ServiceUtils.Close(db, efSession);

                        //allow implementation to send custom message back to WC
                        return OnGetCloseMessage(db, efSession);
                    }
                }
            }

            return $"Session closed for invalid ticket {ticket}";

        }

        protected virtual string OnGetCloseMessage(DbContext db, EfParaquickSession efSession)
        {
            //TODO StopOnErrors? if we didn't stop on errors, do we notify user that errors occured?
            string message= $"Session closed.";

            if (efSession.StatusId==(int)SessionStatuses.Error)
            {
                message += " Errors occurred.";
            }

            return message;
        }

    }
}
