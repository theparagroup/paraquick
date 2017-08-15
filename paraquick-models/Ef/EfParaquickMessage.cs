using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using com.paralib.paraquick.Models;

namespace com.paralib.paraquick.Models.Ef
{
	public partial class EfParaquickMessage:ParaquickMessage
	{
		public virtual EfParaquickMessageType MessageType { get; set;}
		public virtual EfParaquickSession Session { get; set;}
	}
}
