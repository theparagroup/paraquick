using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.paralib.paraquick.qbxml
{
    public interface IRqType
    {
        string requestID { get; set; }
    }

    public partial class CustomerAddRqType:IRqType
    {

    }

}
