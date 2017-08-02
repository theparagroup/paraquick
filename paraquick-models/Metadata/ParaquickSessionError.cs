using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using com.paralib.DataAnnotations;
using com.paralib.paraquick.Models.Metadata;


namespace com.paralib.paraquick.Models
{
	[MetadataType(typeof(ParaquickSessionErrorMetadata))]
	public partial class ParaquickSessionError
	{
	}
}

namespace com.paralib.paraquick.Models.Metadata
{
	public class ParaquickSessionErrorMetadata
	{

		[Key, Column(Order = 0)]
		[Display(Name="Id")]
		[Required(ErrorMessage="Id is required")]
		[ParaType(ParaTypes.Key)]
		public object Id;

		[Display(Name="Session Id")]
		[Required(ErrorMessage="Session Id is required")]
		[ParaType(ParaTypes.Key)]
		public object SessionId;

		[Display(Name="Date")]
		[Required(ErrorMessage="Date is required")]
		[ParaType(ParaTypes.DateTime)]
		public object Date;

		[Display(Name="Message")]
		[ParaType(ParaTypes.LongText)]
		public object Message;
	}
}
