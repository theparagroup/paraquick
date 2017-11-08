using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.paralib.paraquick.qbxml
{
    public interface IEstimate
    {
        CustomerRef CustomerRef { get; set; }
        string Other { get; set; }
        string PONumber { get; set; }
        TermsRef TermsRef { get; set; }
        SalesRepRef SalesRepRef { get; set; }
    }
}
