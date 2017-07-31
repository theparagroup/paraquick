using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.paralib.paraquick.qbxml
{
    public interface IRsType
    {
        string requestID { get; set; }
        string statusCode { get; set; } //0 for success; nonzero for information, warnings, and errors
        string statusSeverity { get; set; } //Info, Warning, Error
        string statusMessage { get; set; }
    }
}
