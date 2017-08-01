using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.paralib.paraquick.qbxml;
using com.paralib.paraquick.Models.Ef;

namespace com.paralib.paraquick.qbwc
{
    public class Request
    {
        public int? Id { protected set; get; }
        public int Sequence { protected set; get; }
        public string RequestId { protected set; get; }
        public DateTime RequestDate { protected set; get; }
        public IRqMsg RqMsg { protected set; get; }
        public IRsMsg RsMsg { protected set; get; }

        public Request(int sequence, IRqMsg rqMsg)
        {
            Sequence = sequence;
            RequestDate = DateTime.Now;
            RqMsg = rqMsg;
            RequestId = rqMsg.requestID;
        }

        internal Request(EfParaquickRequest efRequest)
        {
            Id = efRequest.Id;
            Sequence = efRequest.RequestSequence;
            RequestDate = efRequest.RequestDate;
            RqMsg = (IRqMsg) Msg.Deserialize(System.Type.GetType($"com.paralib.paraquick.qbxml.{efRequest.MessageType},paraquick-qbxml"), efRequest.RequestXml);
            RequestId = RqMsg.requestID;

            //todo reponse stuff

        }

    }
}
