using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using com.paralib.paraquick.Models;

namespace com.paralib.paraquick.Models.Ef
{
	public partial class EfParaquickTicket:ParaquickTicket
	{
		[ForeignKey("CompanyId")]
		public virtual EfParaquickCompany Company { get; set;}
		[ForeignKey("StatusId")]
		public virtual EfParaquickTicketStatus Status { get; set;}
		[InverseProperty("Ticket")]
		public virtual List<EfParaquickRequest> ParaquickRequests { get; set;}
		[InverseProperty("Ticket")]
		public virtual List<EfParaquickTicketError> ParaquickTicketErrors { get; set;}
	}
}
