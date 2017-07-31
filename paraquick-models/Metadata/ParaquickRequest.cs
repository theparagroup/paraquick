using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using com.paralib.DataAnnotations;
using com.paralib.paraquick.Models.Metadata;


namespace com.paralib.paraquick.Models
{
	[MetadataType(typeof(ParaquickRequestMetadata))]
	public partial class ParaquickRequest
	{
	}
}

namespace com.paralib.paraquick.Models.Metadata
{
	public class ParaquickRequestMetadata
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

		[Display(Name="Message Sequence")]
		[Required(ErrorMessage="Message Sequence is required")]
		[ParaType(ParaTypes.Int32)]
		public object MessageSequence;

		[Display(Name="Request Sequence")]
		[Required(ErrorMessage="Request Sequence is required")]
		[ParaType(ParaTypes.Int32)]
		public object RequestSequence;

		[Display(Name="Message Type Id")]
		[Required(ErrorMessage="Message Type Id is required")]
		[ParaType(ParaTypes.Key)]
		public object MessageTypeId;

		[Display(Name="Request Id")]
		[Required(ErrorMessage="Request Id is required")]
		[ParaType(ParaTypes.Name)]
		public object RequestId;

		[Display(Name="Request")]
		[ParaType(ParaTypes.LongText)]
		public object Request;

		[Display(Name="Request Date")]
		[ParaType(ParaTypes.DateTime)]
		public object RequestDate;

		[Display(Name="Response")]
		[ParaType(ParaTypes.LongText)]
		public object Response;

		[Display(Name="Response Date")]
		[ParaType(ParaTypes.DateTime)]
		public object ResponseDate;

		[Display(Name="Status Code")]
		[ParaType(ParaTypes.Int32)]
		public object StatusCode;

		[Display(Name="Status Severity")]
		[ParaType(ParaTypes.Text)]
		public object StatusSeverity;

		[Display(Name="Status Message")]
		[ParaType(ParaTypes.LongText)]
		public object StatusMessage;
	}
}
