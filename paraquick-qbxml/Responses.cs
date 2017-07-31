using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using com.paralib.paraquick.qbxml.Serializers;

namespace com.paralib.paraquick.qbxml
{
    //query, mod, add

   

   
    public partial class CustomerAddRsType : SerializableType, IRsType
    {
        protected override XmlSerializer GetSerializer()
        {
            return new CustomerAddRsTypeSerializer();
        }
    }

    public partial class CustomerModRsType : SerializableType, IRsType
    {
        protected override XmlSerializer GetSerializer()
        {
            return new CustomerModRsTypeSerializer();
        }
    }

    public partial class EstimateAddRsType : SerializableType, IRsType
    {
        protected override XmlSerializer GetSerializer()
        {
            return new EstimateAddRsTypeSerializer();
        }
    }

    public partial class EstimateModRsType : SerializableType, IRsType
    {
        protected override XmlSerializer GetSerializer()
        {
            return new EstimateModRsTypeSerializer();
        }
    }

}
