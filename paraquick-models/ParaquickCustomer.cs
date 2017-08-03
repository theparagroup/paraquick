using System;

namespace com.paralib.paraquick.Models
{
	public partial class ParaquickCustomer
	{
		public int Id { get; set;}
		public int CompanyId { get; set;}
		public string ListId { get; set;}
		public string EditSequence { get; set;}
		public DateTime QueueDate { get; set;}
		public DateTime? CreateDate { get; set;}
		public DateTime? UpdateDate { get; set;}
	}
}
