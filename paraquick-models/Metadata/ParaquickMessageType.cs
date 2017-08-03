using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using com.paralib.DataAnnotations;
using com.paralib.paraquick.Models.Metadata;


namespace com.paralib.paraquick.Models
{
	[MetadataType(typeof(ParaquickMessageTypeMetadata))]
	public partial class ParaquickMessageType
	{
	}
}

namespace com.paralib.paraquick.Models.Metadata
{
	public class ParaquickMessageTypeMetadata
	{

		[Key, Column(Order = 0)]
		[Display(Name="Id")]
		[Required(ErrorMessage="Id is required")]
		[ParaType(ParaTypes.Key)]
		public object Id;

		[Display(Name="Name")]
		[Required(ErrorMessage="Name is required")]
		[ParaType(ParaTypes.Name)]
		public object Name;

		[Display(Name="Request Type Name")]
		[Required(ErrorMessage="Request Type Name is required")]
		[ParaType(ParaTypes.Name)]
		public object RequestTypeName;

		[Display(Name="Response Type Name")]
		[Required(ErrorMessage="Response Type Name is required")]
		[ParaType(ParaTypes.Name)]
		public object ResponseTypeName;
	}
}
