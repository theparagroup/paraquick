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

   
    public partial class CustomerAddRsType : Msg, IRsMsg
    {
        protected override XmlSerializer GetSerializer()
        {
            return new CustomerAddRsTypeSerializer();
        }
    }

    public partial class CustomerQueryRsType : Msg, IRsMsg
    {
        protected override XmlSerializer GetSerializer()
        {
            return new CustomerQueryRsTypeSerializer();
        }
    }

    public partial class CustomerModRsType : Msg, IRsMsg
    {
        protected override XmlSerializer GetSerializer()
        {
            return new CustomerModRsTypeSerializer();
        }
    }

    public partial class EstimateAddRsType : Msg, IRsMsg
    {
        protected override XmlSerializer GetSerializer()
        {
            return new EstimateAddRsTypeSerializer();
        }
    }

    public partial class EstimateQueryRsType : Msg, IRsMsg
    {
        protected override XmlSerializer GetSerializer()
        {
            return new EstimateQueryRsTypeSerializer();
        }
    }

    public partial class EstimateModRsType : Msg, IRsMsg
    {
        protected override XmlSerializer GetSerializer()
        {
            return new EstimateModRsTypeSerializer();
        }
    }

}
