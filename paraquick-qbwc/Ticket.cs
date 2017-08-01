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

        protected List<MessageSet> _messageSets { private set; get; } = new List<MessageSet>();
        protected List<TicketError> _ticketErrors { private set; get; } = new List<TicketError>();

        public int? Id { protected set; get; }
        public Guid Value { protected set; get; }
        public int CompanyId { protected set; get; }
        public DateTime StartDate { protected set; get; }

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

        public MessageSet NewMessageSet(int sequence)
        {
            MessageSet messageSet = new MessageSet(sequence);
            _messageSets.Add(messageSet);
            return messageSet;
        }

        public static Ticket Load(string value)
        {
            Guid guid;

            if (Guid.TryParse(value, out guid))
            {
                using (DbContext db = new DbContext())
                {
                    //load ticket
                    EfParaquickTicket efTicket = (from t in db.ParaquickTickets where t.Value == new Guid(value) select t).First();

                    Ticket ticket = new Ticket(guid);
                    ticket.Id = efTicket.Id;
                    ticket.CompanyId = efTicket.CompanyId;
                    ticket.StartDate = efTicket.StartDate;

                    //load errrors
                    ticket.LoadTicketErrors(db, efTicket);

                    //load requests
                    ticket.LoadMessageSets(db, efTicket);

                    return ticket;
                }

            }
            else
            {
                return null;
            }
        }

        protected void LoadTicketErrors(DbContext db, EfParaquickTicket efTicket)
        {
            _ticketErrors.Clear();

            var efTicketErrors = (from te in db.ParaquickTicketErrors where te.TicketId == efTicket.Id select te).ToList();

            foreach (var efTicketError in efTicketErrors)
            {
                TicketError ticketError = new TicketError();
                ticketError.Id = efTicketError.Id;
                ticketError.Date = efTicketError.Date;
                ticketError.Message = efTicketError.Message;
                _ticketErrors.Add(ticketError);
            }
        }

        protected void LoadMessageSets(DbContext db, EfParaquickTicket efTicket)
        {
            _messageSets.Clear();

            var efRequests = (from r in db.ParaquickRequests where r.TicketId == efTicket.Id orderby r.MessageSequence, r.RequestSequence select r).ToList();

            MessageSet messageSet = null;

            foreach (var efRequest in efRequests)
            {
                if (efRequest.MessageSequence != messageSet?.Sequence)
                {
                    messageSet = new MessageSet(efRequest.MessageSequence);
                    _messageSets.Add(messageSet);
                }

                messageSet.AddRequest(efRequest);
            }
        }

        public void AddError(string message)
        {
            _ticketErrors.Add(new TicketError() { Date = DateTime.Now, Message = message });
        }

        public void Save()
        {
            using (DbContext db = new DbContext())
            {
                EfParaquickTicket efTicket;

                if (Id.HasValue)
                {
                    efTicket = (from t in db.ParaquickTickets where t.Id == Id.Value select t).First();
                }
                else
                {
                    efTicket = new EfParaquickTicket();
                    efTicket.CompanyId = CompanyId;
                    efTicket.Value = Value;
                    efTicket.StartDate = StartDate;
                    efTicket.StatusId = (int)TicketStatuses.New;
                    db.ParaquickTickets.Add(efTicket);
                }

                //add new errors (null id)
                foreach (var te in _ticketErrors)
                {
                    if (!te.Id.HasValue)
                    {
                        EfParaquickTicketError ticketError = new EfParaquickTicketError();
                        ticketError.Ticket = efTicket;
                        ticketError.Date = te.Date;
                        ticketError.Message = te.Message;
                        db.ParaquickTicketErrors.Add(ticketError);
                    }
                }

                //add and/or save requests
                foreach (var ms in _messageSets.OrderBy(ms => ms.Sequence))
                {
                    foreach (var r in ms.Requests.OrderBy(r => r.Sequence))
                    {
                        if (!r.Id.HasValue)
                        {
                            //add
                            EfParaquickRequest efRequest = new EfParaquickRequest();
                            efRequest.Ticket = efTicket;
                            efRequest.MessageSequence = ms.Sequence;
                            efRequest.RequestSequence = r.Sequence;
                            efRequest.RequestId = r.RequestId;
                            efRequest.RequestDate = r.RequestDate;
                            efRequest.MessageType = r.RqMsg?.GetType()?.Name;
                            efRequest.RequestXml = r.RqMsg.Serialize();
                            db.ParaquickRequests.Add(efRequest);

                        }
                        else
                        {
                            //TODO update response
                        }

                    }
                }

                //TODO change status


                db.SaveChanges();

                if (!Id.HasValue)
                {
                    Id = efTicket.Id;
                }

                //reload stuff
                LoadTicketErrors(db, efTicket);
                LoadMessageSets(db, efTicket);

            }
        }

    }
}
