﻿using System;
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

        public long? Id { protected set; get; }
        public string Ticket { protected set; get; }
        public long CompanyId { protected set; get; }
        public DateTime CreateDate { protected set; get; }

        public Session(long companyId)
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
            using (var db = ServiceUtils.CreateDbContext())
            {
                EfParaquickSession efSession = new EfParaquickSession();
                efSession.CompanyId = CompanyId;
                efSession.Ticket = Ticket;
                efSession.CreateDate = CreateDate;
                efSession.StatusId = (long)SessionStatuses.New;

                ServiceUtils.TruncateSession(efSession);

                db.ParaquickSessions.Add(efSession);

                //add  requests
                foreach (var msgSet in _messageSets.OrderBy(ms => ms.Sequence))
                {
                    foreach (var msg in msgSet.Messages.OrderBy(r => r.Sequence))
                    {
                        if (msg.RqMsg!=null)
                        {
                            string rqTypeName = msg.RqMsg.GetType().Name;
                            var efMessageType= (from mt in db.ParaquickMessageTypes where mt.RequestTypeName == rqTypeName select mt).FirstOrDefault();

                            if (efMessageType!=null)
                            {
                                EfParaquickMessage efMessage = new EfParaquickMessage();
                                efMessage.Session = efSession;
                                efMessage.MessageSetSequence = msgSet.Sequence;
                                efMessage.MessageSequence = msg.Sequence;
                                efMessage.ApplicationEntityId = msg.ApplicationEntityId;
                                efMessage.RequestId = msg.RequestId;
                                efMessage.RequestDate = msg.RequestDate;
                                efMessage.MessageType = efMessageType;
                                efMessage.RequestXml = msg.RqMsg.Serialize();

                                ServiceUtils.TruncateMessage(efMessage);

                                db.ParaquickMessages.Add(efMessage);

                            }
                            else
                            {
                                throw new InvalidOperationException($"RqMsg type not found ({rqTypeName})");
                            }

                        }
                        else
                        {
                            throw new InvalidOperationException("RqMsg must have a value");
                        }
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
