using System;

namespace com.paralib.paraquick.Models
{
	public partial class ParaquickSessionError
	{
		public int Id { get; set;}
		public int SessionId { get; set;}
		public DateTime Date { get; set;}
		public string Message { get; set;}
	}
}
