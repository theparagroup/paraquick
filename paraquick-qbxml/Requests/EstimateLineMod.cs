using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.paralib.paraquick.qbxml
{
    public partial class EstimateLineMod : IEstimateLine
    {
        public RateTypes RateType
        {
            set
            {
                ItemElementName = (ItemChoiceType22)value;
            }
            get
            {
                return (RateTypes)ItemElementName;
            }
        }

        public string Rate
        {
            set
            {
                Item = value;
            }
            get
            {
                return Item;
            }
        }

        public MarkupTypes MarkupType
        {
            set
            {
                Item1ElementName = (Item1ChoiceType5)value;
            }
            get
            {
                return (MarkupTypes)Item1ElementName;
            }
        }

        public string Markup
        {
            set
            {
                Item1 = value;
            }
            get
            {
                return Item1?.ToString();
            }
        }
    }
}
