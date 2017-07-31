using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.paralib.paraquick.Models.Ef;

namespace com.paralib.paraquick.qbwc
{
    public class Ticket
    {
        /*
            The "zero ticket" is the ticket returned when there is nothing to do, or an error.
        */
        public static readonly Ticket Zero = new Ticket(new Guid("00000000-0000-0000-0000-000000000000"));

        protected class TicketError
        {
            public DateTime Date;
            public string Message;
        }

        public List<RequestMessageSet> RequestMessageSets { private set; get; } = new List<RequestMessageSet>();
        protected List<TicketError> _errors { private set; get; } = new List<TicketError>();

        public int? Id { protected set; get; }
        public Guid Value { protected set; get; }
        public int CompanyId { protected set; get; }
        public DateTime StartDate { protected set; get; }
        public DateTime? EndDate { protected set; get; }

        protected Ticket(Guid value)
        {
            Value = value;
        }

        public static Ticket New(int companyId)
        {
            Ticket ticket = new Ticket(Guid.NewGuid());
            ticket.Id = null;
            ticket.CompanyId = companyId;
            ticket.StartDate = DateTime.Now;
            return ticket;
        }

        public static Ticket Load(string value)
        {
            //RequestMessageSets.Clear();
            //_errors.Clear();
            return null;
        }

        public void AddError(string message)
        {
            _errors.Add(new TicketError() { Date = DateTime.Now, Message = message });
        }

        public void Save()
        {
            using (DbContext db = new DbContext())
            {
                EfParaquickTicket  ticket;

                if (Id.HasValue)
                {
                    ticket = (from t in db.ParaquickTickets where t.Id == Id.Value select t).First();
                    //we can only add errors or change statuses on edit?
                }
                else
                {
                    ticket = new EfParaquickTicket();
                    ticket.CompanyId = CompanyId;
                    ticket.Value = Value;
                    ticket.StartDate = StartDate;
                    ticket.StatusId = (int)TicketStatuses.New;
                    db.ParaquickTickets.Add(ticket);

                    //save message sets/requests

                    db.SaveChanges();
                }

                //add errors with null id
                _errors.Clear();

                //any ticket or response errors (via query(, set status to error
                //ticket.StatusId = ticketStatus;

                db.SaveChanges();

            }
        }

    }
}
