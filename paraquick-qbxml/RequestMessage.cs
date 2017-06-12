using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.paralib.paraquick.qbxml
{
    public class RequestMessage : Message, IEnumerable<IRqType>
    {
        protected List<IRqType> _requests { get; set; } = new List<IRqType>();
        protected Dictionary<string, IRqType> _ids { get; set; } = new Dictionary<string, IRqType>();

        public RequestMessage(QBXMLMsgsRqOnError onError= QBXMLMsgsRqOnError.stopOnError)
        {
            OnError = onError;
        }

        public QBXMLMsgsRqOnError OnError { get; set; }

        public void Add(string id, IRqType request)
        {
            request.requestID = id;
            _requests.Add(request);
            _ids.Add(id, request);
        }

        public void Clear()
        {
            _requests.Clear();
            _ids.Clear();
        }

        public int Count
        {
            get
            {
                return _requests.Count;
            }
        }

        public IRqType this[string id]
        {
            get
            {
                return _ids[id];
            }
        }

        public IEnumerator<IRqType> GetEnumerator()
        {
            return _requests.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected override void OnDeserialize(QBXML qbxml)
        {
            //typically we would never read these requests, but it might be handy for testing, etc.
            throw new NotImplementedException();
        }

        protected override QBXML OnSerialize()
        {
            QBXMLMsgsRq msgRq = new QBXMLMsgsRq();
            msgRq.onError = OnError;

            List<IRqType> rqs = new List<IRqType>();

            //request order matters!
            foreach (var rq in _requests)
            {
                rqs.Add(rq);
            }

            msgRq.Items = rqs.ToArray();

            QBXML qbxml = new QBXML() { Items = new[] { msgRq }, ItemsElementName = new[] { ItemsChoiceType99.QBXMLMsgsRq } };

            return qbxml;
        }

    }
}
