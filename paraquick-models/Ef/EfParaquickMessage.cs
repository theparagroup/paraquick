using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using com.paralib.paraquick.Models;

namespace com.paralib.paraquick.Models.Ef
{
	public partial class EfParaquickMessage:ParaquickMessage
	{
		[ForeignKey("MessageTypeId")]
		public virtual EfParaquickMessageType MessageType { get; set;}
		[ForeignKey("SessionId")]
		public virtual EfParaquickSession Session { get; set;}
	}
}
