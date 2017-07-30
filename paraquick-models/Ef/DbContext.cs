using System;
using System.Data.Entity;
using com.paralib.Dal.Ef;
using para=com.paralib.Ado;

namespace com.paralib.paraquick.Models.Ef
{
	[DbConfigurationType(typeof(EfConfiguration))]
	public partial class DbContext:EfContext
	{

		public DbContext() { _init(); }
		public DbContext(string connectionString) : base(connectionString) { _init(); }
		public DbContext(para.Database database) : base(database) { _init(); }

		private void _init()
		{
			 OnInit();
		}

		protected virtual void OnInit()
		{
			#if DEBUG
			Database.Log = message => System.Diagnostics.Debug.WriteLine(message);
			#endif
		}

		public DbSet<EfParaquickCompany> ParaquickCompanies { get; set; }
	}
}
