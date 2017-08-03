using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using com.paralib.paraquick.Models;

namespace com.paralib.paraquick.Models.Ef
{
	public partial class EfParaquickMessageType:ParaquickMessageType
	{
		[InverseProperty("RequestMessageType")]
		public virtual List<EfParaquickMessage> ParaquickMessages { get; set;}
	}
}
