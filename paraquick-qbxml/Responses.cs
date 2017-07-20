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

    public interface IRsType
    {
        string requestID { get; set; }
        string operation { get; }
        string statusCode { get; set; } //0 for success; nonzero for information, warnings, and errors
        string statusSeverity { get; set; } //Info, Warning, Error
        string statusMessage { get; set; }
    }

   
    public partial class CustomerAddRsType : SerializableType, IRsType
    {
        public string operation => "CustomerAdd";

        protected override XmlSerializer GetSerializer()
        {
            return new CustomerAddRsTypeSerializer();
        }
    }

    public partial class CustomerModRsType : SerializableType, IRsType
    {
        public string operation => "CustomerMod";

        protected override XmlSerializer GetSerializer()
        {
            return new CustomerModRsTypeSerializer();
        }
    }

}
