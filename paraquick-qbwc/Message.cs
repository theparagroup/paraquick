using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.paralib.paraquick.qbxml;
using com.paralib.paraquick.Models.Ef;

namespace com.paralib.paraquick.qbwc
{
    public class Message
    {
        public int? Id { protected set; get; }
        public int Sequence { protected set; get; }
        public int ApplicationEntityId { protected set; get; }
        public string RequestId { protected set; get; }
        public DateTime RequestDate { protected set; get; }
        public IRqMsg RqMsg { protected set; get; }
        public IRsMsg RsMsg { protected set; get; }

        public Message(int sequence, int applicationEntityId, IRqMsg rqMsg)
        {
            Sequence = sequence;
            ApplicationEntityId = applicationEntityId;
            RequestDate = DateTime.Now;
            RqMsg = rqMsg;
            RequestId = rqMsg.requestID;
        }

        internal Message(EfParaquickMessage efMessage)
        {
            Id = efMessage.Id;
            Sequence = efMessage.MessageSequence;
            ApplicationEntityId = efMessage.ApplicationEntityId;
            RequestDate = efMessage.RequestDate;
            RqMsg = (IRqMsg)Msg.Deserialize(efMessage.MessageType.RequestTypeName, efMessage.RequestXml);
            RequestId = RqMsg.requestID;
        }


    }
}
