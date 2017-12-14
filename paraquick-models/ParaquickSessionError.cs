using System;

namespace com.paralib.paraquick.Models
{
	public partial class ParaquickSessionError
	{
		public long Id { get; set;}
		public long SessionId { get; set;}
		public DateTime Date { get; set;}
		public string Message { get; set;}
	}
}
