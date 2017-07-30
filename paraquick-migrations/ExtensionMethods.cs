using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.paralib.Migrations;
using com.paralib.DataAnnotations;

namespace com.paralib.paraquick.Migrations
{
    public static class ExtensionMethods
    {
        public static void ParaquickTables(this FluentMigrator.Builders.Create.ICreateExpressionRoot Create)
        {
            Create.Table("paraquick_companies")
                .WithColumn("id").AsParaType(ParaTypes.Key).PrimaryKey().Identity()
                .WithColumn("name").AsParaType(ParaTypes.Name)
                .WithColumn("path").AsParaType(ParaTypes.Path)
                .WithColumn("user_name").AsParaType(ParaTypes.Name)
                .WithColumn("password").AsParaType(ParaTypes.Name);
        }

        public static void ParaquickTables(this FluentMigrator.Builders.Delete.IDeleteExpressionRoot Delete)
        {
            Delete.Table("paraquick_companies");

        }

    }
}
