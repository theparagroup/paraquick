using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using com.paralib.paraquick.Models;

namespace com.paralib.paraquick.Models.Ef
{
	public partial class EfParaquickSessionStatus:ParaquickSessionStatus
	{
		[InverseProperty("Status")]
		public virtual List<EfParaquickSession> ParaquickSessions { get; set;}
	}
}
