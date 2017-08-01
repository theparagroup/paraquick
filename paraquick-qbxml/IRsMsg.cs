using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace com.paralib.paraquick.qbxml
{
    public interface IRsMsg:IMsg
    {
        string statusCode { get; set; } //0 for success; nonzero for information, warnings, and errors
        string statusSeverity { get; set; } //Info, Warning, Error
        string statusMessage { get; set; }
    }
}
