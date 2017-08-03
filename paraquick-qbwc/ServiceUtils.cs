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

        internal static EfParaquickSession FindNextSession(DbContext db, int companyId)
        {
            //Find a "new" session for this company and change its status to "processing"
            return (from s in db.ParaquickSessions where s.CompanyId == companyId && s.StatusId == (int)SessionStatuses.New orderby s.CreateDate select s).FirstOrDefault();
        }

        internal static List<EfParaquickMessage> FindNextMessageSet(DbContext db, EfParaquickSession efSession)
        {
            //Find the next messageset sequence for this session (we go by ResponseDate) - can be null
            var messageSetSequence= (from msg in db.ParaquickMessages where msg.SessionId == efSession.Id && msg.ResponseDate==null orderby msg.MessageSetSequence, msg.MessageSequence select msg.MessageSetSequence).FirstOrDefault();

            //get the messages for the set - can be empty list
            var efMessages=(from msg in db.ParaquickMessages where msg.MessageSetSequence==messageSetSequence orderby msg.MessageSetSequence, msg.MessageSequence select msg).ToList();

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



        internal static void Open(DbContext db, EfParaquickSession efSession)
        {
            efSession.StatusId = (int)SessionStatuses.Open;
            efSession.StartDate = DateTime.Now;
            db.SaveChanges();
        }

        internal static void Reset(DbContext db, EfParaquickSession efSession)
        {
            efSession.StatusId = (int)SessionStatuses.New;
            efSession.StartDate = null;
            db.SaveChanges();
        }

        internal static void Response()
        {
            //TODO update message with response
        }

        internal static void Error(DbContext db, EfParaquickSession efSession, string message)
        {
            EfParaquickSessionError sessionError = new EfParaquickSessionError();
            sessionError.Session = efSession;
            sessionError.Date = DateTime.Now;
            sessionError.Message = message;
            db.ParaquickSessionErrors.Add(sessionError);

            db.SaveChanges();
        }


        internal static void Close(DbContext db, EfParaquickSession efSession)
        {
            //change status to success or error based on session errors and response status codes

            if (efSession.ParaquickMessages.Where(m => m.StatusCode == null).Count() > 0)
            {
                efSession.StatusId = (int)SessionStatuses.Incomplete;
            }
            else if (efSession.ParaquickSessionErrors.Count > 0)
            {
                efSession.StatusId = (int)SessionStatuses.Error;
            }
            else if (efSession.ParaquickMessages.Where(m => (m.StatusCode ?? 0) != 0).Count() > 0)
            {
                efSession.StatusId = (int)SessionStatuses.Error;
            }
            else
            {
                efSession.StatusId = (int)SessionStatuses.Success;
            }

            efSession.EndDate = DateTime.Now;
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
                if ((msg.StatusCode ?? 0) != 0)
                {
                    message += $" [Request ({msg.Id}) : {msg.StatusCode} - {msg.StatusMessage}]";
                }
            }

            return message;
        }


    }
}
