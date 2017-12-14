using System;

namespace com.paralib.paraquick.Models
{
	public partial class ParaquickSession
	{
		public long Id { get; set;}
		public long CompanyId { get; set;}
		public string Ticket { get; set;}
		public DateTime CreateDate { get; set;}
		public DateTime? StartDate { get; set;}
		public DateTime? EndDate { get; set;}
		public long StatusId { get; set;}
	}
}
