using System;

namespace com.paralib.paraquick.Models
{
	public partial class ParaquickRequest
	{
		public int Id { get; set;}
		public int TicketId { get; set;}
		public int MessageSequence { get; set;}
		public int RequestSequence { get; set;}
		public string MessageType { get; set;}
		public string RequestId { get; set;}
		public string RequestXml { get; set;}
		public DateTime RequestDate { get; set;}
		public string ResponseXml { get; set;}
		public DateTime? ResponseDate { get; set;}
		public int? StatusCode { get; set;}
		public string StatusSeverity { get; set;}
		public string StatusMessage { get; set;}
	}
}
