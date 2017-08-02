using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.paralib.paraquick.qbxml;
using com.paralib.paraquick.Models.Ef;

namespace com.paralib.paraquick.qbwc
{
    public class MessageSet
    {
        public int Sequence { protected set; get; }
        protected List<Message> _messages = new List<Message>();

        public IEnumerable<Message> Messages => _messages;

        public MessageSet(int sequence)
        {
            Sequence = sequence;
        }

        public MessageSet(EfParaquickMessage efMessage)
        {
            Sequence = efMessage.MessageSetSequence;
        }

        public Message NewMessage(int sequence, IRqMsg rqMsg)
        {
            Message message = new Message(sequence, rqMsg);
            _messages.Add(message);
            return message;
        }

        internal Message AddMessage(EfParaquickMessage efMessage)
        {
            Message message = new Message(efMessage);
            _messages.Add(message);
            return message;
        }



    }
}
