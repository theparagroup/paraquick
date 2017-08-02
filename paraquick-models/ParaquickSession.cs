using System;

namespace com.paralib.paraquick.Models
{
	public partial class ParaquickSession
	{
		public int Id { get; set;}
		public int CompanyId { get; set;}
		public string Ticket { get; set;}
		public DateTime CreateDate { get; set;}
		public DateTime? StartDate { get; set;}
		public DateTime? EndDate { get; set;}
		public int StatusId { get; set;}
	}
}
