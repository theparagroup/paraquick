using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using com.paralib.paraquick.Models;

namespace com.paralib.paraquick.Models.Ef
{
	public partial class EfParaquickCompany:ParaquickCompany
	{
		[InverseProperty("Company")]
		public virtual List<EfParaquickTicket> ParaquickTickets { get; set;}
		[InverseProperty("Company")]
		public virtual List<EfParaquickCustomer> ParaquickCustomers { get; set;}
		[InverseProperty("Company")]
		public virtual List<EfParaquickEstimate> ParaquickEstimates { get; set;}
	}
}
