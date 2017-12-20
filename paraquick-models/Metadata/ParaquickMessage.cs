using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using com.paralib.DataAnnotations;
using com.paralib.paraquick.Models.Metadata;


namespace com.paralib.paraquick.Models
{
	[MetadataType(typeof(ParaquickMessageMetadata))]
	public partial class ParaquickMessage
	{
	}
}

namespace com.paralib.paraquick.Models.Metadata
{
	public class ParaquickMessageMetadata
	{

		[Key]
		[Display(Name="Id")]
		[Required(ErrorMessage="Id is required")]
		[ParaType(ParaTypes.Key)]
		public object Id;

		[ForeignKey("Session")]
		[Display(Name="Session Id")]
		[Required(ErrorMessage="Session Id is required")]
		[ParaType(ParaTypes.Key)]
		public object SessionId;

		[Display(Name="Message Set Sequence")]
		[Required(ErrorMessage="Message Set Sequence is required")]
		[ParaType(ParaTypes.Int32)]
		public object MessageSetSequence;

		[Display(Name="Message Sequence")]
		[Required(ErrorMessage="Message Sequence is required")]
		[ParaType(ParaTypes.Int32)]
		public object MessageSequence;

		[ForeignKey("MessageType")]
		[Display(Name="Message Type Id")]
		[Required(ErrorMessage="Message Type Id is required")]
		[ParaType(ParaTypes.Key)]
		public object MessageTypeId;

		[Display(Name="Application Entity Id")]
		[Required(ErrorMessage="Application Entity Id is required")]
		[ParaType(ParaTypes.Key)]
		public object ApplicationEntityId;

		[Display(Name="Request Id")]
		[Required(ErrorMessage="Request Id is required")]
		[ParaType(ParaTypes.GuidString)]
		public object RequestId;

		[Display(Name="Request Xml")]
		[Required(ErrorMessage="Request Xml is required")]
		[ParaType(ParaTypes.Max)]
		public object RequestXml;

		[Display(Name="Request Date")]
		[Required(ErrorMessage="Request Date is required")]
		[ParaType(ParaTypes.DateTime)]
		public object RequestDate;

		[Display(Name="Response Xml")]
		[ParaType(ParaTypes.Max)]
		public object ResponseXml;

		[Display(Name="Response Date")]
		[ParaType(ParaTypes.DateTime)]
		public object ResponseDate;

		[Display(Name="Status Code")]
		[ParaType(ParaTypes.Description)]
		public object StatusCode;

		[Display(Name="Status Severity")]
		[ParaType(ParaTypes.Note)]
		public object StatusSeverity;

		[Display(Name="Status Message")]
		[ParaType(ParaTypes.LongText)]
		public object StatusMessage;
	}
}
