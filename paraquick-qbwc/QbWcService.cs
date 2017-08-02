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
        protected override string OnAuthenticate(string username, string password, out AuthCodes authCode, out AuthOptions authOptions)
        {

            using (DbContext db = new DbContext())
            {
                //Find the company based on the unique user name
                EfParaquickCompany efCompany = (from c in db.ParaquickCompanies where c.UserName == username && c.Password == password select c).FirstOrDefault();

                authOptions = new AuthOptions(efCompany.Path);

                if (efCompany != null)
                {
                    EfParaquickSession efSession = SessionUtils.FindNewSession(db, efCompany.Id);

                    //work to do?
                    if (efSession != null)
                    {
                        //allow implementors to override
                        authCode= OnNewSession(efSession.Ticket, authOptions);

                        if (authCode==AuthCodes.VALID)
                        {
                            //open and return ticket
                            SessionUtils.Open(db,efSession);
                            return efSession.Ticket;
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
                    Logger.Error($"username {username} and password {password} incorrect");
                    authCode = AuthCodes.NVU;
                }


                authOptions = null;
                return SessionUtils.ZeroTicket;
            }
        }

        protected virtual AuthCodes OnNewSession(string ticket, AuthOptions authOptions)
        {
            //allow implementation to set busy or change authoptions...
            return AuthCodes.VALID;
        }


        protected override string OnConnectionError(string ticket, HResult hResult)
        {
            using (DbContext db = new DbContext())
            {
                EfParaquickSession efSession = SessionUtils.FindSession(db, ticket);

                if (efSession!=null)
                {
                    SessionUtils.Error(db, efSession, hResult.Format());
                }
                else
                {
                    Logger.Error("can't find ticket [{ticket}]");
                }

                return "done";
            }
        }

        protected override string OnCreateRequestMessage(string ticket, string hcpXml, string companyFilePath, string qbCountry, int qbMajorVersion, int qbMinorVersion)
        {
            if (ticket == SessionUtils.ZeroTicket)
            {
                //return empty work message
                var requestMessage = new RqMsgSet();
                string xml = requestMessage.Serialize();
                return xml;
            }
            else
            {
                using (DbContext db = new DbContext())
                {
                    EfParaquickSession efSession = SessionUtils.FindSession(db, ticket);

                    if (efSession != null)
                    {
                        //confirm file path
                        if (IsCompanyFileValid(null, companyFilePath))
                        {
                            //TODO find next message for this ticket

                            return "xml";
                        }
                        else
                        {
                            //log error
                            SessionUtils.Error(db, efSession, $"bad company file [{companyFilePath}] for company [{efSession.CompanyId}]");
                        }
                    }
                    else
                    {
                        //bad tickets will recur in getLastError and be logged there
                    }

                }


            }

            //error condition
            return "";
        }

        protected abstract bool IsCompanyFileValid(string ticket, string companyFilePath);

        protected override int OnResponseMessage(string ticket, string responseXml, HResult hResult)
        {
            //TODO will the zero ticket ever appear here?

            //TODO
            //process responses & update entities based on response type
            OnResponse();

            //update status and report "%"

            //do we stop on errors here or keep going?
            //do we send a -1 when we're really done?


            throw new NotImplementedException();
        }

        protected abstract void OnResponse();

        protected override string OnGetLastError(string ticket)
        {
            //TODO will the zero ticket ever appear here?


            //TODO
            Logger.Error("can't find ticket [{ticket}]");


            //return response-level and ticket-level errors
            return base.OnGetLastError(ticket);
        }


        protected override string OnCloseConnection(string ticket)
        {
            //TODO will the zero ticket ever appear here?


            //TODO
            //if we didn't stop on errors, do we notify user that errors occured?
            return base.OnCloseConnection(ticket);
        }

    }
}
