using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using com.paralib.paraquick.qbxml.Serializers;

namespace com.paralib.paraquick.qbxml
{
   

    public partial class CustomerAddRqType : Msg, IRqMsg
    {
        protected override XmlSerializer GetSerializer()
        {
            return new CustomerAddRqTypeSerializer();
        }
    }

    public partial class CustomerQueryRqType : Msg, IRqMsg
    {
        protected override XmlSerializer GetSerializer()
        {
            return new CustomerQueryRqTypeSerializer();
        }
    }

    public partial class CustomerModRqType : Msg, IRqMsg
    {
        protected override XmlSerializer GetSerializer()
        {
            return new CustomerModRqTypeSerializer();
        }
    }

    public partial class EstimateAddRqType : Msg, IRqMsg
    {
        protected override XmlSerializer GetSerializer()
        {
            return new EstimateAddRqTypeSerializer();
        }
    }

    public partial class EstimateQueryRqType : Msg, IRqMsg
    {
        protected override XmlSerializer GetSerializer()
        {
            return new EstimateQueryRqTypeSerializer();
        }
    }

    public partial class EstimateModRqType : Msg, IRqMsg
    {
        protected override XmlSerializer GetSerializer()
        {
            return new EstimateModRqTypeSerializer();
        }
    }


}
