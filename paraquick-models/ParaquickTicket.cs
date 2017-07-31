using System;

namespace com.paralib.paraquick.Models
{
	public partial class ParaquickTicket
	{
		public int Id { get; set;}
		public int CompanyId { get; set;}
		public Guid Value { get; set;}
		public DateTime StartDate { get; set;}
		public DateTime? EndDate { get; set;}
		public int StatusId { get; set;}
	}
}
