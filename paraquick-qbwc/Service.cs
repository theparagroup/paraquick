using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;

namespace com.paralib.paraquick.qbwc
{
    /* 
        The following is required for the web connector

            [WebService(Namespace = "http://developer.intuit.com/")]

    */
    [WebService(Namespace = "http://developer.intuit.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public abstract class Service: WebService
    {
        /*
            Note: you should mark all of the implemented methods with 

                [WebMethod()]
        */


        public abstract string[] authenticate(string strUserName, string strPassword);

        public abstract string clientVersion(string strVersion);

        public abstract string serverVersion();

        public abstract string closeConnection(string ticket);

        public abstract string connectionError(string ticket, string hresult, string message);

        public abstract string getLastError(string ticket);

        public abstract string sendRequestXML(string ticket, string strHCPResponse, string strCompanyFileName, string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers);

        public abstract int receiveResponseXML(string ticket, string response, string hresult, string message);

        public abstract string getInteractiveURL(string wcTicket, string sessionID);

        public abstract string interactiveDone(string wcTicket);

        public abstract string interactiveRejected(string wcTicket, string reason);

    }
}



