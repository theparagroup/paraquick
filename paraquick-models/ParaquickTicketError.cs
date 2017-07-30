using System;

namespace com.paralib.paraquick.Models
{
	public partial class ParaquickTicketError
	{
		public int Id { get; set;}
		public int TicketId { get; set;}
		public DateTime Date { get; set;}
		public string Message { get; set;}
	}
}
