using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.paralib.paraquick.Models.Ef;

namespace com.paralib.paraquick.qbwc
{

    public class Session
    {
        protected List<MessageSet> _messageSets { private set; get; } = new List<MessageSet>();

        public int? Id { protected set; get; }
        public string Ticket { protected set; get; }
        public int CompanyId { protected set; get; }
        public DateTime CreateDate { protected set; get; }

        public Session(int companyId)
        {
            CompanyId = companyId;
            Ticket= Guid.NewGuid().ToString();
            CreateDate = DateTime.Now;
        }

        public MessageSet NewMessageSet(int sequence)
        {
            MessageSet messageSet = new MessageSet(sequence);
            _messageSets.Add(messageSet);
            return messageSet;
        }


        public void Save()
        {
            using (DbContext db = new DbContext())
            {
                EfParaquickSession efSession = new EfParaquickSession();
                efSession.CompanyId = CompanyId;
                efSession.Ticket = Ticket;
                efSession.CreateDate = CreateDate;
                efSession.StatusId = (int)SessionStatuses.New;
                db.ParaquickSessions.Add(efSession);

                //add  requests
                foreach (var msgSet in _messageSets.OrderBy(ms => ms.Sequence))
                {
                    foreach (var msg in msgSet.Messages.OrderBy(r => r.Sequence))
                    {
                        EfParaquickMessage efMessage = new EfParaquickMessage();
                        efMessage.Session = efSession;
                        efMessage.MessageSetSequence = msgSet.Sequence;
                        efMessage.MessageSequence = msg.Sequence;
                        efMessage.RequestId = msg.RequestId;
                        efMessage.RequestDate = msg.RequestDate;
                        efMessage.RequestMessageType = msg.RqMsg?.GetType()?.Name;
                        efMessage.RequestXml = msg.RqMsg.Serialize();
                        db.ParaquickMessages.Add(efMessage);
                    }
                }


                db.SaveChanges();

                //refresh stuff
                Id = efSession.Id;
                LoadMessageSets(db, efSession);


            }
        }

        protected void LoadMessageSets(DbContext db, EfParaquickSession efSession)
        {
            _messageSets.Clear();

            var efMessages = (from m in db.ParaquickMessages where m.SessionId == efSession.Id orderby m.MessageSetSequence, m.MessageSequence select m).ToList();

            MessageSet messageSet = null;

            foreach (var efMessage in efMessages)
            {
                if (efMessage.MessageSetSequence != messageSet?.Sequence)
                {
                    messageSet = new MessageSet(efMessage);
                    _messageSets.Add(messageSet);
                }

                messageSet.AddMessage(efMessage);
            }
        }

    }
}
