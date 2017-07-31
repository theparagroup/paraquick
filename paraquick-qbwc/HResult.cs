using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.paralib.paraquick.qbwc
{
    public class HResult
    {
        public string Code { protected set; get; }
        public string Message { protected set; get; }

        public HResult(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Format()
        {
            return $"[{Code}] : [{Message}]";
        }

    }
}
