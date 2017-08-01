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
        protected List<Request> _requests = new List<Request>();

        public IEnumerable<Request> Requests => _requests;

        public MessageSet(int sequence)
        {
            Sequence = sequence;
        }

        public Request NewRequest(int sequence, IRqMsg rqMsg)
        {
            Request request = new Request(sequence, rqMsg);
            _requests.Add(request);
            return request;
        }

        internal Request AddRequest(EfParaquickRequest efRequest)
        {
            Request request = new Request(efRequest);
            _requests.Add(request);
            return request;
        }



    }
}
