using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.paralib.paraquick.qbxml;

namespace com.paralib.paraquick.qbwc
{
    public class RequestMessageSet
    {
        public List<IRqType> Requests { private set; get; } = new List<IRqType>();

    }
}
