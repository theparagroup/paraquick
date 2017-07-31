using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.paralib.paraquick.qbwc
{
    public abstract class QbWcService : QbWcServiceBase
    {

        protected override string OnConnectionError(string ticketValue, string hresult, string message)
        {
            //Log error on ticket. 

            return "done";
        }

        protected override Ticket OnCreateTicket(string userName, string password, out TicketCodes code, out string companyFilePath)
        {
            //Find the company based on the unique user name
            //Find a "new" ticket and change its status to "processing"

            throw new NotImplementedException();
        }

        protected override string OnCreateRequestMessage(string ticketValue, string companyFilePath)
        {
            //confirm file path
            //find a new message for this ticket

            throw new NotImplementedException();
        }

        protected bool IsCompanyFileValid(Ticket ticket, string companyFilePath)
        {
            return true;
        }

        protected override int OnResponseMessage(string ticketValue, string responseXml, HResult hresult)
        {
            //process responses & update entities based on response type
            OnResponse();

            //update status and report "%"

            //do we stop on errors here or keep going?
            //do we send a -1 when we're really done?


            throw new NotImplementedException();
        }

        protected abstract void OnResponse();

        protected override string OnGetLastError(string ticketValue)
        {
            //return response-level and ticket-level errors
            return base.OnGetLastError(ticketValue);
        }

        protected override string OnCloseConnection(string ticketValue)
        {
            //if we didn't stop on errors, do we notify user that errors occured?
            return base.OnCloseConnection(ticketValue);
        }

    }
}
