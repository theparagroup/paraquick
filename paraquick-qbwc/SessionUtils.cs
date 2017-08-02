using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.paralib.paraquick.Models.Ef;

namespace com.paralib.paraquick.qbwc
{

    public class SessionUtils
    {
        /*
            The "zero ticket" is the ticket returned when there is nothing to do, or an error.
        */
        public const string ZeroTicket = "00000000-0000-0000-0000-000000000000";

        internal static EfParaquickSession FindNewSession(DbContext db, int companyId)
        {
            //Find a "new" session for this company and change its status to "processing"
            return (from s in db.ParaquickSessions where s.CompanyId == companyId && s.StatusId == (int)SessionStatuses.New orderby s.CreateDate select s).FirstOrDefault();
        }

        internal static EfParaquickSession FindSession(DbContext db, string ticket)
        {
            //find session by ticket
            return (from s in db.ParaquickSessions where s.Ticket == ticket select s).FirstOrDefault();
        }


        internal static void Open(DbContext db, EfParaquickSession efSession)
        {
            efSession.StatusId = (int)SessionStatuses.Processing;
            efSession.StartDate = DateTime.Now;
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
            //TODO change status to success or error based on session errors and response status codes


            efSession.EndDate = DateTime.Now;
            db.SaveChanges();

        }


    }
}
