using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using com.paralib.paraquick.qbxml.Serializers;

namespace com.paralib.paraquick.qbxml
{
   

    public partial class CustomerAddRqType : SerializableType, IRqType
    {
        protected override XmlSerializer GetSerializer()
        {
            return new CustomerAddRqTypeSerializer();
        }
    }

    public partial class CustomerModRqType : SerializableType, IRqType
    {
        protected override XmlSerializer GetSerializer()
        {
            return new CustomerModRqTypeSerializer();
        }
    }

    public partial class EstimateAddRqType : SerializableType, IRqType
    {
        protected override XmlSerializer GetSerializer()
        {
            return new EstimateAddRqTypeSerializer();
        }
    }

    public partial class EstimateModRqType : SerializableType, IRqType
    {
        protected override XmlSerializer GetSerializer()
        {
            return new EstimateModRqTypeSerializer();
        }
    }


}
