using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using com.paralib.DataAnnotations;
using com.paralib.paraquick.Models.Metadata;


namespace com.paralib.paraquick.Models
{
	[MetadataType(typeof(ParaquickSessionMetadata))]
	public partial class ParaquickSession
	{
	}
}

namespace com.paralib.paraquick.Models.Metadata
{
	public class ParaquickSessionMetadata
	{

		[Key]
		[Display(Name="Id")]
		[Required(ErrorMessage="Id is required")]
		[ParaType(ParaTypes.Key)]
		public object Id;

		[ForeignKey("Company")]
		[Display(Name="Company Id")]
		[Required(ErrorMessage="Company Id is required")]
		[ParaType(ParaTypes.Key)]
		public object CompanyId;

		[Display(Name="Ticket")]
		[Required(ErrorMessage="Ticket is required")]
		[ParaType(ParaTypes.GuidString)]
		public object Ticket;

		[Display(Name="Create Date")]
		[Required(ErrorMessage="Create Date is required")]
		[ParaType(ParaTypes.DateTime)]
		public object CreateDate;

		[Display(Name="Start Date")]
		[ParaType(ParaTypes.DateTime)]
		public object StartDate;

		[Display(Name="End Date")]
		[ParaType(ParaTypes.DateTime)]
		public object EndDate;

		[ForeignKey("Status")]
		[Display(Name="Status Id")]
		[Required(ErrorMessage="Status Id is required")]
		[ParaType(ParaTypes.Key)]
		public object StatusId;
	}
}
