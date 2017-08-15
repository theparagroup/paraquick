using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using com.paralib.paraquick.Models;

namespace com.paralib.paraquick.Models.Ef
{
	public partial class EfParaquickSession:ParaquickSession
	{
		public virtual EfParaquickCompany Company { get; set;}
		public virtual EfParaquickSessionStatus Status { get; set;}

		[InverseProperty("Session")]
		public virtual List<EfParaquickMessage> ParaquickMessages { get; set;}
		[InverseProperty("Session")]
		public virtual List<EfParaquickSessionError> ParaquickSessionErrors { get; set;}
	}
}
