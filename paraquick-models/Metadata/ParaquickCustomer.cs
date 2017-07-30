using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using com.paralib.DataAnnotations;
using com.paralib.paraquick.Models.Metadata;


namespace com.paralib.paraquick.Models
{
	[MetadataType(typeof(ParaquickCustomerMetadata))]
	public partial class ParaquickCustomer
	{
	}
}

namespace com.paralib.paraquick.Models.Metadata
{
	public class ParaquickCustomerMetadata
	{

		[Key, Column(Order = 0)]
		[Display(Name="Id")]
		[Required(ErrorMessage="Id is required")]
		[ParaType(ParaTypes.Key)]
		public object Id;

		[Display(Name="Company Id")]
		[Required(ErrorMessage="Company Id is required")]
		[ParaType(ParaTypes.Key)]
		public object CompanyId;

		[Display(Name="List Id")]
		[ParaType(ParaTypes.Text)]
		public object ListId;

		[Display(Name="Edit Sequence")]
		[ParaType(ParaTypes.Text)]
		public object EditSequence;
	}
}
