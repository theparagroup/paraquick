using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;

namespace com.paralib.paraquick.qbwc
{
    [WebService(Namespace = "http://developer.intuit.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    // [System.Web.Script.Services.ScriptService]
    public abstract class Service: WebService
    {
        [WebMethod()]
        public abstract string[] authenticate(string strUserName, string strPassword);

        [WebMethod()]
        public abstract string clientVersion(string strVersion);

        [WebMethod()]
        public abstract string serverVersion();

        [WebMethod()]
        public abstract string closeConnection(string ticket);

        [WebMethod()]
        public abstract string connectionError(string ticket, string hresult, string message);

        [WebMethod()]
        public abstract string getLastError(string ticket);

        [WebMethod()]
        public abstract string sendRequestXML(string ticket, string strHCPResponse, string strCompanyFileName, string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers);

        [WebMethod()]
        public abstract int receiveResponseXML(string ticket, string response, string hresult, string message);

        [WebMethod()]
        public abstract string getInteractiveURL(string wcTicket, string sessionID);

        [WebMethod()]
        public abstract string interactiveDone(string wcTicket);

        [WebMethod()]
        public abstract string interactiveRejected(string wcTicket, string reason);

    }
}



