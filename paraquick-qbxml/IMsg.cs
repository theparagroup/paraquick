using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace com.paralib.paraquick.qbxml
{
    public interface IMsg
    {
        string requestID { get; set; }
        string Serialize();
    }
}
