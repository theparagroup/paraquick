using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using com.paralib.DataAnnotations;
using com.paralib.paraquick.Models.Metadata;


namespace com.paralib.paraquick.Models
{
	[MetadataType(typeof(ParaquickTicketMetadata))]
	public partial class ParaquickTicket
	{
	}
}

namespace com.paralib.paraquick.Models.Metadata
{
	public class ParaquickTicketMetadata
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

		[Display(Name="Value")]
		[Required(ErrorMessage="Value is required")]
		public object Value;

		[Display(Name="Start Date")]
		[Required(ErrorMessage="Start Date is required")]
		[ParaType(ParaTypes.DateTime)]
		public object StartDate;

		[Display(Name="End Date")]
		[ParaType(ParaTypes.DateTime)]
		public object EndDate;

		[Display(Name="Status Id")]
		[Required(ErrorMessage="Status Id is required")]
		[ParaType(ParaTypes.Key)]
		public object StatusId;
	}
}
