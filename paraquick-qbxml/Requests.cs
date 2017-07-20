using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using com.paralib.paraquick.qbxml.Serializers;

namespace com.paralib.paraquick.qbxml
{
    public interface IRqType
    {
        string requestID { get; set; }
        string operation { get; }
    }

    public partial class CustomerAddRqType : SerializableType, IRqType
    {
        public string operation => "CustomerAdd";

        protected override XmlSerializer GetSerializer()
        {
            return new CustomerAddRqTypeSerializer();
        }
    }

    public partial class CustomerModRqType : SerializableType, IRqType
    {
        public string operation => "CustomerMod";

        protected override XmlSerializer GetSerializer()
        {
            return new CustomerModRqTypeSerializer();
        }
    }

}
