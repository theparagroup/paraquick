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
    }

    public partial class CustomerAddRqType : SerializableType, IRqType
    {
        protected override XmlSerializer GetSerializer()
        {
            return new CustomerAddRqTypeSerializer();
        }
    }

}
