using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using com.paralib.DataAnnotations;
using com.paralib.paraquick.Models.Metadata;


namespace com.paralib.paraquick.Models
{
	[MetadataType(typeof(ParaquickTicketErrorMetadata))]
	public partial class ParaquickTicketError
	{
	}
}

namespace com.paralib.paraquick.Models.Metadata
{
	public class ParaquickTicketErrorMetadata
	{

		[Key, Column(Order = 0)]
		[Display(Name="Id")]
		[Required(ErrorMessage="Id is required")]
		[ParaType(ParaTypes.Key)]
		public object Id;

		[Display(Name="Ticket Id")]
		[Required(ErrorMessage="Ticket Id is required")]
		[ParaType(ParaTypes.Key)]
		public object TicketId;

		[Display(Name="Date")]
		[Required(ErrorMessage="Date is required")]
		[ParaType(ParaTypes.DateTime)]
		public object Date;

		[Display(Name="Message")]
		[ParaType(ParaTypes.LongText)]
		public object Message;
	}
}
