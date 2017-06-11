using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.paralib.paraquick.qbxml
{
    public class RequestMessage:Message
    {
        //public List<RqType> Requests { get; protected set; } = new List<RqType>();
        public List<object> Requests = new List<object>();

        protected override QBXML OnRoot()
        {
            QBXMLMsgsRq msgRq = new QBXMLMsgsRq();
            msgRq.onError = QBXMLMsgsRqOnError.stopOnError;
            msgRq.Items = Requests.ToArray();

            QBXML qbxml = new QBXML() { Items = new[] { msgRq }, ItemsElementName = new[] { ItemsChoiceType99.QBXMLMsgsRq } };

            return qbxml;
        }
    }
}
