using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using com.paralib.paraquick.Models;

namespace com.paralib.paraquick.Models.Ef
{
	public partial class EfParaquickRequest:ParaquickRequest
	{
		[ForeignKey("MessageTypeId")]
		public virtual EfParaquickMessageType MessageType { get; set;}
		[ForeignKey("TicketId")]
		public virtual EfParaquickTicket Ticket { get; set;}
	}
}
