using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.paralib.paraquick.qbwc
{
    public enum SessionStatuses : long
    {
        New = 1,
        Open = 2,
        Incomplete = 3,
        Success = 4,
        Error = 5
    }
}
