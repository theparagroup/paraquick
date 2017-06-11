using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.paralib.paraquick.qbxml
{
    public class ResponseMessage : Message, IEnumerable<IRsType>
    {
        protected List<IRsType> _responses { get; set; } = new List<IRsType>();
        protected Dictionary<string, IRsType> _ids { get; set; } = new Dictionary<string, IRsType>();

        public void Add(string id, IRsType response)
        {
            response.requestID = id;
            _responses.Add(response);
            _ids.Add(id, response);
        }

        public void Clear()
        {
            _responses.Clear();
            _ids.Clear();
        }

        public IRsType this[string id]
        {
            get
            {
                return _ids[id];
            }
        }

        public IEnumerator<IRsType> GetEnumerator()
        {
            return _responses.GetEnumerator();
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

            QBXMLMsgsRs rsMsg = (QBXMLMsgsRs) qbxml.Items[0];

            foreach (var obj in rsMsg.Items)
            {
                if (obj is IRsType)
                {
                    var rs = (IRsType)obj;

                    if (rs.requestID==null)
                    {
                        rs.requestID = Guid.NewGuid().ToString();
                    }

                    _responses.Add(rs);
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
