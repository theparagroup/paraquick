using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.paralib.paraquick.Models.Ef;
using com.paralib.paraquick.qbxml;

namespace com.paralib.paraquick.qbwc
{

    public class ServiceUtils
    {
        /*
            The "zero ticket" is the ticket returned from authenticate whenever the authcode is anything other than VALID
        */
        public const string ZeroTicket = "00000000-0000-0000-0000-000000000000";

        public static RqMsgSet DoNothingRqMsgSet()
        {
            //this is handy for testing
            return new RqMsgSet();
        }

        public static DbContext CreateDbContext()
        {
            return new DbContext(Paralib.Dal.Databases["paraquick"]);
        }

        internal static EfParaquickSession FindSession(DbContext db, string ticket)
        {
            //find session by ticket
            return (from s in db.ParaquickSessions where s.Ticket == ticket select s).FirstOrDefault();
        }

        internal static EfParaquickSession FindNextSession(DbContext db, long companyId)
        {
            //Find a "new" session for this company and change its status to "processing"
            return (from s in db.ParaquickSessions where s.CompanyId == companyId && s.StatusId == (long)SessionStatuses.New orderby s.CreateDate select s).FirstOrDefault();
        }

        internal static List<EfParaquickMessage> FindNextMessageSet(DbContext db, EfParaquickSession efSession)
        {
            //Find the next messageset sequence for this session (we go by ResponseDate) - can be null
            long? messageSetSequence = efSession.ParaquickMessages.Where(m => m.ResponseDate == null).OrderBy(m => m.MessageSetSequence).ThenBy(m => m.MessageSequence).FirstOrDefault()?.MessageSetSequence;

            //get the messages for the set - can be empty list
            var efMessages = efSession.ParaquickMessages.Where(m => m.MessageSetSequence == messageSetSequence).OrderBy(m => m.MessageSetSequence).ThenBy(m => m.MessageSequence).ToList();


            //OLD

            //Find the next messageset sequence for this session (we go by ResponseDate) - can be null
            //var xmessageSetSequence= (from msg in db.ParaquickMessages where msg.SessionId == efSession.Id && msg.ResponseDate==null orderby msg.MessageSetSequence, msg.MessageSequence select msg.MessageSetSequence).First();

            //get the messages for the set - can be empty list
            //var efMessages=(from msg in db.ParaquickMessages where msg.MessageSetSequence==messageSetSequence orderby msg.MessageSetSequence, msg.MessageSequence select msg).ToList();

            //verify that the set has not been partially processed for some reason
            foreach (var efMessage in efMessages)
            {
                if (efMessage.ResponseDate!=null && efMessage.ResponseXml!=null && efMessage.StatusCode!=null)
                {
                    return null;
                }
            }

            return efMessages;
        }


        public static void TruncateSession(EfParaquickSession efSession)
        {
            DataAnnotations.ObjectTruncator.Truncate(efSession);
            DataAnnotations.ObjectTruncator.Truncate(efSession.Company);
        }

        public static void TruncateSessionError(EfParaquickSessionError sessionError)
        {
            DataAnnotations.ObjectTruncator.Truncate(sessionError);
        }

        public static void TruncateMessage(EfParaquickMessage efMessage)
        {
            DataAnnotations.ObjectTruncator.Truncate(efMessage);
        }

        internal static void Session(DbContext db, EfParaquickSession efSession, string hcpXml, string qbCountry, int? qbMajorVersion, int? qbMinorVersion)
        {
            efSession.Company.HcpXml = hcpXml;
            efSession.Company.Country = qbCountry;
            efSession.Company.Major = qbMajorVersion;
            efSession.Company.Minor = qbMinorVersion;

            TruncateSession(efSession);

            db.SaveChanges();
        }

        internal static void SessionError(DbContext db, EfParaquickSession efSession, string message)
        {
            EfParaquickSessionError sessionError = new EfParaquickSessionError();
            sessionError.Session = efSession;
            sessionError.Date = DateTime.Now;
            sessionError.Message = message;

            TruncateSessionError(sessionError);

            db.ParaquickSessionErrors.Add(sessionError);

            db.SaveChanges();
        }

        internal static void Open(DbContext db, EfParaquickSession efSession)
        {
            efSession.StatusId = (long)SessionStatuses.Open;
            efSession.StartDate = DateTime.Now;

            TruncateSession(efSession);

            db.SaveChanges();
        }

        internal static void Reset(DbContext db, EfParaquickSession efSession)
        {
            efSession.StatusId = (long)SessionStatuses.New;
            efSession.StartDate = null;

            TruncateSession(efSession);

            db.SaveChanges();

        }

        internal static void Request(DbContext db, EfParaquickMessage efMessage, IRqMsg rqMsg)
        {
            //re-serialize
            efMessage.RequestXml = rqMsg.Serialize();

            //truncate
            TruncateMessage(efMessage);

            //save
            db.SaveChanges();
        }

        internal static void RequestError(DbContext db, EfParaquickMessage efMessage, string errorMessage)
        {
            //update message with non-Qb (application defined) error
            efMessage.ResponseDate = DateTime.Now;
            efMessage.StatusCode = "-1";
            efMessage.StatusMessage = errorMessage;

            TruncateMessage(efMessage);

            db.SaveChanges();

        }

        internal static void Response(DbContext db, EfParaquickMessage efMessage, IRsMsg rsMsg)
        {
            //update message with response
            efMessage.ResponseDate = DateTime.Now;
            efMessage.ResponseXml = rsMsg.Serialize();
            efMessage.StatusCode = rsMsg.statusCode;
            efMessage.StatusSeverity = rsMsg.statusSeverity;
            efMessage.StatusMessage = rsMsg.statusMessage;

            TruncateMessage(efMessage);

            db.SaveChanges();

        }


        internal static int CalculatePercentComplete(DbContext db, EfParaquickSession efSession)
        {
            int complete = efSession.ParaquickMessages.Where(m => m.ResponseDate != null).ToList().Count;
            int total = efSession.ParaquickMessages.Count;
            int pctComplete;

            if (complete == total)
            {
                pctComplete = 100;
            }
            else
            {
                var fPc = ((float)complete) / total;
                pctComplete = (int)(fPc * 100);

                //excessive but certain
                if (pctComplete <= 0)
                {
                    pctComplete = 1;
                }

                //excessive but certain
                if (pctComplete >= 100)
                {
                    pctComplete = 99;
                }

            }

            return pctComplete;
        }





        internal static void Close(DbContext db, EfParaquickSession efSession)
        {
            //change status to success or error based on session errors and response status codes

            if (efSession.ParaquickMessages.Where(m => m.StatusCode == null).Count() > 0)
            {
                efSession.StatusId = (long)SessionStatuses.Incomplete;
            }
            else if (efSession.ParaquickSessionErrors.Count > 0)
            {
                efSession.StatusId = (long)SessionStatuses.Error;
            }
            else if (efSession.ParaquickMessages.Where(m => m.StatusCode != "0").Count() > 0)
            {
                efSession.StatusId = (long)SessionStatuses.Error;
            }
            else
            {
                efSession.StatusId = (long)SessionStatuses.Success;
            }

            efSession.EndDate = DateTime.Now;

            TruncateSession(efSession);

            db.SaveChanges();

        }

        public static string FormatErrors(EfParaquickSession efSession)
        {
            //format response-level and ticket-level errors

            string message = "Errors occurred.";

            foreach (var se in efSession.ParaquickSessionErrors)
            {
                message += $" [Session ({se.SessionId}) : {se.Message}]";
            }

            foreach (var msg in efSession.ParaquickMessages)
            {
                if (msg.StatusCode != "0")
                {
                    message += $" [Request ({msg.Id}) : {msg.StatusCode} - {msg.StatusMessage}]";
                }
            }

            return message;
        }


    }
}
