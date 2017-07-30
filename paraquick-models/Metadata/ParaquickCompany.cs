using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using com.paralib.DataAnnotations;
using com.paralib.paraquick.Models.Metadata;


namespace com.paralib.paraquick.Models
{
	[MetadataType(typeof(ParaquickCompanyMetadata))]
	public partial class ParaquickCompany
	{
	}
}

namespace com.paralib.paraquick.Models.Metadata
{
	public class ParaquickCompanyMetadata
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

		[Display(Name="Path")]
		[Required(ErrorMessage="Path is required")]
		[ParaType(ParaTypes.Path)]
		public object Path;

		[Display(Name="User Name")]
		[Required(ErrorMessage="User Name is required")]
		[ParaType(ParaTypes.Name)]
		public object UserName;

		[Display(Name="Password")]
		[Required(ErrorMessage="Password is required")]
		[ParaType(ParaTypes.Name)]
		public object Password;
	}
}
