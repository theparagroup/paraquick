using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;
using System.Xml.Serialization;
using com.paralib.paraquick.qbxml;

namespace com.paralib.paraquick.qbwc
{
    /* 
        The following is required on your implementation:

            [WebService(Namespace = "http://developer.intuit.com/")]

        These are common attributes to add as well:

            [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
            [System.ComponentModel.ToolboxItem(false)]


    */
    public abstract class QbWcServiceBase : WebService
    {

        [WebMethod()]
        public string serverVersion()
        {
            /* 
                WC asking for our version.
                
                Return:
                
                    "whatever you want"
            */

            return OnServerVersion();
        }

        protected virtual string OnServerVersion()
        {
            return "Paraquick QuickBooks Web Connector Service";
        }


        [WebMethod()]
        public string clientVersion(string strVersion)
        {
            /*
            
                WC informing us of it's version. 
                
                Example: 
                
                    "2.2.0.71"

                Return:

                    "" - proceed
                    "W:message" - let the user decide
                    "E:message" - stop
                    "O:version" - stop and display version required (must be at least major.minor)

            */

            return OnClientVersion(strVersion);
        }

        protected virtual string OnClientVersion(string version)
        {
            return "";
        }

        [WebMethod()]
        public string[] authenticate(string strUserName, string strPassword)
        {
            /* 
                WC starting session. 
             
                Return strings:

                    0: ticket
                    1:  
                        NONE - "no data exchange required"
                        NVU - invalid user
                        BUSY
                        "<company file path>"
                        "" - use open qb
                    2: postpone seconds
                    3: minimum for every minutes
                    4: minimum for every second

            */


            TicketCodes code;
            string companyFilePath;
            Ticket ticket = OnCreateTicket(strUserName, strPassword, out code, out companyFilePath);

            string[] response = { ticket.Value.ToString(), null };

            switch (code)
            {
                case TicketCodes.VALID:
                    response[1] = companyFilePath;
                    break;
                case TicketCodes.NONE:
                    response[1] = "NONE";
                    break;
                case TicketCodes.NVU:
                    response[1] = "NVU";
                    break;
                case TicketCodes.BUSY:
                    response[1] = "BUSY";
                    break;
            }

            return response;


        }

        protected abstract Ticket OnCreateTicket(string userName, string password, out TicketCodes code, out string companyFilePath);

        [WebMethod()]
        public string connectionError([XmlElement(ElementName = "ticket")]string strTicket, string hresult, string message)
        {
            /*
                WC reporting error connecting to quickbooks or the company file
            
                Return 

                    "done" - stop 
                    "<path to company file>" - retry 
            */

            return OnConnectionError(strTicket, hresult, message);
        }

        protected virtual string OnConnectionError(string ticketValue, string hresult, string message)
        {
            return "done";
        }

        [WebMethod()]
        public string sendRequestXML([XmlElement(ElementName = "ticket")]string strTicket, string strHCPResponse, string strCompanyFileName, string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers)
        {
            //valid request (could have no operations)
            //"" for error (getLastError will be called - documentation is wrong!)

            return OnCreateRequestMessage(strTicket, strCompanyFileName);
        }

        protected abstract string OnCreateRequestMessage(string ticketValue, string companyFilePath);

        [WebMethod()]
        public int receiveResponseXML([XmlElement(ElementName = "ticket")]string strTicket, [XmlElement(ElementName = "response")]string strResponse, [XmlElement(ElementName = "hresult")]string strHresult, [XmlElement(ElementName = "message")]string strMessage)
        {
            //0-100 : % complete
            //<0    : error (getLastError will be called)

            HResult hresult = null;

            if (strHresult!="")
            {
                hresult = new HResult(strHresult, strMessage);
            }

            return OnResponseMessage(strTicket, strResponse, hresult);
        }

        protected abstract int OnResponseMessage(string ticketValue, string responseXml, HResult hresult);


        [WebMethod()]
        public string getLastError([XmlElement(ElementName = "ticket")]string strTicket)
        {
            //called when receiveResponseXML returns negative number
            //or when sendRequestXML returns ""
            //any message except "Interactive mode", which starts interactive mode

            return OnGetLastError(strTicket);
        }

        protected virtual string OnGetLastError(string ticketValue)
        {
            return "";
        }


        [WebMethod()]
        public string closeConnection(string ticket)
        {
            //wc informing us session is over
            //return final message to user

            return OnCloseConnection(ticket);

        }

        protected virtual string OnCloseConnection(string ticketValue)
        {
            return "";
        }


        [WebMethod()]
        public string getInteractiveURL(string wcTicket, string sessionID)
        {
            return "https://google.com/foo";
        }

        [WebMethod()]
        public string interactiveDone(string wcTicket)
        {
            return "done";
        }

        [WebMethod()]
        public string interactiveRejected(string wcTicket, string reason)
        {
            return "";
        }

    }
}



