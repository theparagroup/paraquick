using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.paralib.paraquick.qbxml
{
    public interface IEstimateLine
    {
        ItemRef ItemRef { get; set; }
        string Desc { get; set; }
        string Quantity { get; set; }
        RateTypes RateType { get; set; }
        string Rate { get; set; }
        MarkupTypes MarkupType { get; set; }
        string Markup { get; set; }
        string Other1 { get; set; }
    }
}
