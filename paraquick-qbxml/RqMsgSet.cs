using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.paralib.paraquick.qbxml
{
    public class RqMsgSet : MsgSet, IEnumerable<IRqMsg>
    {
        protected List<IRqMsg> _rqMsgs { get; set; } = new List<IRqMsg>();
        protected Dictionary<string, IRqMsg> _ids { get; set; } = new Dictionary<string, IRqMsg>();

        public RqMsgSet(QBXMLMsgsRqOnError onError= QBXMLMsgsRqOnError.stopOnError)
        {
            OnError = onError;
        }

        public QBXMLMsgsRqOnError OnError { get; set; }

        public void Add(IRqMsg rqMsg)
        {
            if (string.IsNullOrEmpty(rqMsg.requestID))
            {
                throw new InvalidOperationException("Request must have an id");
            }

            _rqMsgs.Add(rqMsg);
            _ids.Add(rqMsg.requestID, rqMsg);
        }

        public void Clear()
        {
            _rqMsgs.Clear();
            _ids.Clear();
        }

        public int Count
        {
            get
            {
                return _rqMsgs.Count;
            }
        }

        public IRqMsg this[string id]
        {
            get
            {
                return _ids[id];
            }
        }

        public IEnumerator<IRqMsg> GetEnumerator()
        {
            return _rqMsgs.GetEnumerator();
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
            QBXMLMsgsRq msgRqSet = new QBXMLMsgsRq();
            msgRqSet.onError = OnError;

            List<IRqMsg> rqMsgs = new List<IRqMsg>();

            //request order matters!
            foreach (var rq in _rqMsgs)
            {
                rqMsgs.Add(rq);
            }

            msgRqSet.Items = rqMsgs.ToArray();

            QBXML qbxml = new QBXML() { Items = new[] { msgRqSet }, ItemsElementName = new[] { ItemsChoiceType99.QBXMLMsgsRq } };

            return qbxml;
        }


        

    }
}
