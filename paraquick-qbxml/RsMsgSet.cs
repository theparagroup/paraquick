using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.paralib.paraquick.qbxml
{
    public class RsMsgSet : MsgSet, IEnumerable<IRsMsg>
    {
        protected List<IRsMsg> _rsMsgs { get; set; } = new List<IRsMsg>();
        protected Dictionary<string, IRsMsg> _ids { get; set; } = new Dictionary<string, IRsMsg>();

        public void Add(IRsMsg rsMsg)
        {
            if (string.IsNullOrEmpty(rsMsg.requestID))
            {
                throw new InvalidOperationException("Response must have an id");
            }

            _rsMsgs.Add(rsMsg);
            _ids.Add(rsMsg.requestID, rsMsg);
        }

        public void Clear()
        {
            _rsMsgs.Clear();
            _ids.Clear();
        }

        public int Count
        {
            get
            {
                return _rsMsgs.Count;
            }
        }

        public IRsMsg this[string id]
        {
            get
            {
                return _ids[id];
            }
        }

        public IEnumerator<IRsMsg> GetEnumerator()
        {
            return _rsMsgs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected override QBXML OnSerialize()
        {
            //typically we would never generate these responses, but it might be handy for testing, etc.
            throw new NotImplementedException();
        }

        protected override void OnDeserialize(QBXML qbxml)
        {
            Clear();

            QBXMLMsgsRs rsMsgSet = (QBXMLMsgsRs)qbxml.Items[0];

            if (rsMsgSet.Items != null)
            {
                //order doesn't "matter" here, but we preserve it
                foreach (var obj in rsMsgSet.Items)
                {
                    if (obj is IRsMsg)
                    {
                        var rs = (IRsMsg)obj;

                        if (rs.requestID == null)
                        {
                            rs.requestID = Guid.NewGuid().ToString();
                        }

                        _rsMsgs.Add(rs);
                        _ids.Add(rs.requestID, rs);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }

        }

    }
}
