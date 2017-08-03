using System;

namespace com.paralib.paraquick.Models
{
	public partial class ParaquickMessage
	{
		public int Id { get; set;}
		public int SessionId { get; set;}
		public int MessageSetSequence { get; set;}
		public int MessageSequence { get; set;}
		public int MessageTypeId { get; set;}
		public int ApplicationEntityId { get; set;}
		public string RequestId { get; set;}
		public string RequestXml { get; set;}
		public DateTime RequestDate { get; set;}
		public string ResponseXml { get; set;}
		public DateTime? ResponseDate { get; set;}
		public string StatusCode { get; set;}
		public string StatusSeverity { get; set;}
		public string StatusMessage { get; set;}
	}
}
