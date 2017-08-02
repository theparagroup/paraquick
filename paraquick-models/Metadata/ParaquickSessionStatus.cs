using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using com.paralib.DataAnnotations;
using com.paralib.paraquick.Models.Metadata;


namespace com.paralib.paraquick.Models
{
	[MetadataType(typeof(ParaquickSessionStatusMetadata))]
	public partial class ParaquickSessionStatus
	{
	}
}

namespace com.paralib.paraquick.Models.Metadata
{
	public class ParaquickSessionStatusMetadata
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
	}
}
