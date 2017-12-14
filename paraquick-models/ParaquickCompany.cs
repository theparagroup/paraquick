using System;

namespace com.paralib.paraquick.Models
{
	public partial class ParaquickCompany
	{
		public long Id { get; set;}
		public string Name { get; set;}
		public string UserName { get; set;}
		public string Password { get; set;}
		public string HcpXml { get; set;}
		public string Country { get; set;}
		public int? Major { get; set;}
		public int? Minor { get; set;}
	}
}
