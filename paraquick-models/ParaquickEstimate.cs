using System;

namespace com.paralib.paraquick.Models
{
	public partial class ParaquickEstimate
	{
		public int Id { get; set;}
		public int CompanyId { get; set;}
		public string TxnId { get; set;}
		public string EditSequence { get; set;}
	}
}
