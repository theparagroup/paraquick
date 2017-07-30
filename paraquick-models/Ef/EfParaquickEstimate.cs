using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using com.paralib.paraquick.Models;

namespace com.paralib.paraquick.Models.Ef
{
	public partial class EfParaquickEstimate:ParaquickEstimate
	{
		[ForeignKey("CompanyId")]
		public virtual EfParaquickCompany Company { get; set;}
	}
}
