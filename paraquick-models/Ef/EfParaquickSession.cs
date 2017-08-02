using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using com.paralib.paraquick.Models;

namespace com.paralib.paraquick.Models.Ef
{
	public partial class EfParaquickSession:ParaquickSession
	{
		[ForeignKey("CompanyId")]
		public virtual EfParaquickCompany Company { get; set;}
		[ForeignKey("StatusId")]
		public virtual EfParaquickSessionStatus Status { get; set;}
		[InverseProperty("Session")]
		public virtual List<EfParaquickMessage> ParaquickMessages { get; set;}
		[InverseProperty("Session")]
		public virtual List<EfParaquickSessionError> ParaquickSessionErrors { get; set;}
	}
}
