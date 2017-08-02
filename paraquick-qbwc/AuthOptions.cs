using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.paralib.paraquick.qbwc
{
    public class AuthOptions
    {
        public string CompanyFilePath { protected set; get; }
        public int? PostponeSeconds { protected set; get; }
        public int? RunEveryMinuteMinimum { protected set; get; }
        public int? RunEverySecondMinimum { protected set; get; }

        public AuthOptions(string companyFilePath="", int? postponeSeconds=null, int? runEveryMinuteMinimum=null, int? runEverySecondMinimum=null)
        {
            CompanyFilePath = companyFilePath;
            PostponeSeconds = postponeSeconds;
            RunEveryMinuteMinimum = runEveryMinuteMinimum;
            RunEverySecondMinimum = runEverySecondMinimum;
        }
    }
}
