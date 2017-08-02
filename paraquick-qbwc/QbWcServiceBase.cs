﻿using System;
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

        Basic implementation of the QbWc service. 
        
        Used as the base for our paraquick implementation as well as for custom versions, or testing.
        
        The following is required on the concrete implementation (*.asmx):

            [WebService(Namespace = "http://developer.intuit.com/")]

        These are common attributes to add as well:

            [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
            [System.ComponentModel.ToolboxItem(false)]


    */
    public abstract class QbWcServiceBase : WebService
    {
        protected ILog Logger { private set; get;  } = Paralib.GetLogger(typeof(QbWcServiceBase));


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
        public string clientVersion([XmlElement(ElementName = "strVersion")] string strClientVersion)
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

            string message;
            VersionCodes versionCode = OnClientVersion(strClientVersion, out message);

            switch (versionCode)
            {
                case VersionCodes.WARNING:
                    return $"W:{message}";

                case VersionCodes.ERROR:
                    return $"E:{message}";

                case VersionCodes.OKAY:
                    return $"O:{message}";
            }

            //VALID
            return "";

        }

        protected virtual VersionCodes OnClientVersion(string clientVersion, out string message)
        {
            message = null;
            return VersionCodes.VALID;
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
                    3: minimum autorun for every minutes
                    4: minimum autorun for every seconds

            */


            AuthCodes authCode;
            AuthOptions authOptions;
            string ticket = OnAuthenticate(strUserName, strPassword, out authCode, out authOptions);

            List<string> response = new List<string>();

            //0: ticket
            response.Add(ticket);

            //1: code or companyfile (can be blank)
            switch (authCode)
            {
                case AuthCodes.VALID:
                    response.Add(authOptions?.CompanyFilePath??"");
                    break;
                case AuthCodes.NONE:
                    response.Add("NONE");
                    break;
                case AuthCodes.NVU:
                    response.Add("NVU");
                    break;
                case AuthCodes.BUSY:
                    response.Add("BUSY");
                    break;
            }

            if (authOptions != null)
            {
                //2: postpone seconds
                if (authOptions.PostponeSeconds.HasValue) response.Add(authOptions.PostponeSeconds.ToString());

                //4: minimum autorun minutes (in seconds)
                if (authOptions.RunEveryMinuteMinimum.HasValue) response.Add(authOptions.RunEveryMinuteMinimum.ToString());

                //5: minimum autorun seconds
                if (authOptions.RunEverySecondMinimum.HasValue) response.Add(authOptions.RunEverySecondMinimum.ToString());
            }

            return response.ToArray();

        }

        protected abstract string OnAuthenticate(string username, string password, out AuthCodes authCode, out AuthOptions authOptions);

        [WebMethod()]
        public string connectionError([XmlElement(ElementName = "ticket")]string strTicket, [XmlElement(ElementName = "hresult")]string strHresult, [XmlElement(ElementName = "message")]string strMessage)
        {
            /*
                WC reporting error connecting to quickbooks or the company file
            
                Return 

                    "done" - stop 
                    "<path to company file>" - retry 
            */

            return OnConnectionError(strTicket, new HResult(strHresult, strMessage));
        }

        protected virtual string OnConnectionError(string ticket, HResult hResult)
        {
            return "done";
        }

        [WebMethod()]
        public string sendRequestXML([XmlElement(ElementName = "ticket")]string strTicket, string strHCPResponse, string strCompanyFileName, string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers)
        {
            /*

                WC asking for work.

                HCP:                HostQuery, CompanyQuery, PreferencesQuery (first request of session only)
                CompanyFileName:    Path to the company file used in the session
                Country:            QuickBooks product country
                Major:              QuickBooks product major version
                Minor:              QuickBooks product minor version

                Return:
                    valid QBXML request (could have no operations)
                    "" for error (getLastError will be called - documentation is wrong!)

            */

            return OnCreateRequestMessage(strTicket, strHCPResponse, strCompanyFileName, qbXMLCountry, qbXMLMajorVers, qbXMLMinorVers);
        }

        protected abstract string OnCreateRequestMessage(string ticket, string hcpXml, string companyFilePath, string qbCountry, int qbMajorVersion, int qbMinorVersion);

        [WebMethod()]
        public int receiveResponseXML([XmlElement(ElementName = "ticket")]string strTicket, [XmlElement(ElementName = "response")]string strResponse, [XmlElement(ElementName = "hresult")]string strHresult, [XmlElement(ElementName = "message")]string strMessage)
        {
            //0-100 : % complete
            //<0    : error (getLastError will be called)

            return OnResponseMessage(strTicket, strResponse, (strHresult == "" ? null : new HResult(strHresult, strMessage)));
        }

        protected abstract int OnResponseMessage(string ticket, string responseXml, HResult hResult);


        [WebMethod()]
        public string getLastError([XmlElement(ElementName = "ticket")]string strTicket)
        {
            //called when receiveResponseXML returns negative number
            //or when sendRequestXML returns ""
            //any message except "Interactive mode", which starts interactive mode

            return OnGetLastError(strTicket);
        }

        protected virtual string OnGetLastError(string ticket)
        {
            return "";
        }


        [WebMethod()]
        public string closeConnection([XmlElement(ElementName = "ticket")]string strTicket)
        {
            //wc informing us session is over
            //return final message to user

            return OnCloseConnection(strTicket);

        }

        protected virtual string OnCloseConnection(string ticket)
        {
            return "";
        }


        /*

            We're not supporting interactive mode in this version.

        */

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


