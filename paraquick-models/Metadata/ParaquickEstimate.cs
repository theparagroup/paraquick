using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using com.paralib.DataAnnotations;
using com.paralib.paraquick.Models.Metadata;


namespace com.paralib.paraquick.Models
{
	[MetadataType(typeof(ParaquickEstimateMetadata))]
	public partial class ParaquickEstimate
	{
	}
}

namespace com.paralib.paraquick.Models.Metadata
{
	public class ParaquickEstimateMetadata
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

		[Display(Name="Txn Id")]
		[ParaType(ParaTypes.Text)]
		public object TxnId;

		[Display(Name="Edit Sequence")]
		[ParaType(ParaTypes.Text)]
		public object EditSequence;

		[Display(Name="Create Date")]
		[Required(ErrorMessage="Create Date is required")]
		[ParaType(ParaTypes.DateTime)]
		public object CreateDate;

		[Display(Name="Update Date")]
		[ParaType(ParaTypes.DateTime)]
		public object UpdateDate;
	}
}
