using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.paralib.Migrations;
using com.paralib.DataAnnotations;

using FluentMigrator.Builders.Insert;

namespace com.paralib.paraquick.Migrations
{
    public static class ExtensionMethods
    {
        public static void ParaquickTables(this FluentMigrator.Builders.Create.ICreateExpressionRoot Create, FluentMigrator.Migration migration)
        {
            Create.Table("paraquick_companies")
                .WithColumn("id").AsParaType(ParaTypes.Key).PrimaryKey().Identity()
                .WithColumn("name").AsParaType(ParaTypes.Name)
                .WithColumn("path").AsParaType(ParaTypes.Path)
                .WithColumn("user_name").AsParaType(ParaTypes.Name).Unique()
                .WithColumn("password").AsParaType(ParaTypes.Name);

            Create.Table("paraquick_customers")
                 .WithColumn("id").AsParaType(ParaTypes.Key).PrimaryKey().Identity()
                 .WithColumn("company_id").AsParaType(ParaTypes.Key).ForeignKey("paraquick_companies", "id")
                 .WithColumn("list_id").AsParaType(ParaTypes.Text).Nullable() //36
                 .WithColumn("edit_sequence").AsParaType(ParaTypes.Text).Nullable(); //16

            Create.Table("paraquick_estimates")
                 .WithColumn("id").AsParaType(ParaTypes.Key).PrimaryKey().Identity()
                 .WithColumn("company_id").AsParaType(ParaTypes.Key).ForeignKey("paraquick_companies", "id")
                 .WithColumn("txn_id").AsParaType(ParaTypes.Text).Nullable() //36
                 .WithColumn("edit_sequence").AsParaType(ParaTypes.Text).Nullable(); //16

            Create.Table("paraquick_ticket_statuses")
                .WithColumn("id").AsParaType(ParaTypes.Key).PrimaryKey()
                .WithColumn("name").AsParaType(ParaTypes.Name);

            migration.Insert.IntoTable("paraquick_ticket_statuses").Row(new { id = 1, name = "New" });
            migration.Insert.IntoTable("paraquick_ticket_statuses").Row(new { id = 2, name = "Processing" });
            migration.Insert.IntoTable("paraquick_ticket_statuses").Row(new { id = 3, name = "Success" });
            migration.Insert.IntoTable("paraquick_ticket_statuses").Row(new { id = 4, name = "Error" });

            Create.Table("paraquick_tickets")
                .WithColumn("id").AsParaType(ParaTypes.Key).PrimaryKey().Identity()
                .WithColumn("company_id").AsParaType(ParaTypes.Key).ForeignKey("paraquick_companies", "id")
                .WithColumn("value").AsGuid()
                .WithColumn("start_date").AsParaType(ParaTypes.DateTime)
                .WithColumn("end_date").AsParaType(ParaTypes.DateTime).Nullable()
                .WithColumn("status_id").AsParaType(ParaTypes.Key).ForeignKey("paraquick_ticket_statuses", "id");

            Create.Table("paraquick_ticket_errors")
                .WithColumn("id").AsParaType(ParaTypes.Key).PrimaryKey().Identity()
                .WithColumn("ticket_id").AsParaType(ParaTypes.Key).ForeignKey("paraquick_tickets", "id")
                .WithColumn("date").AsParaType(ParaTypes.DateTime)
                .WithColumn("message").AsParaType(ParaTypes.LongText).Nullable();

            Create.Table("paraquick_requests")
               .WithColumn("id").AsParaType(ParaTypes.Key).PrimaryKey().Identity()
               .WithColumn("ticket_id").AsParaType(ParaTypes.Key).ForeignKey("paraquick_tickets", "id")
               .WithColumn("message_sequence").AsParaType(ParaTypes.Int32)
               .WithColumn("request_sequence").AsParaType(ParaTypes.Int32)
               .WithColumn("message_type").AsParaType(ParaTypes.Text)
               .WithColumn("request_id").AsParaType(ParaTypes.Name)
               .WithColumn("request_xml").AsParaType(ParaTypes.LongText)
               .WithColumn("request_date").AsParaType(ParaTypes.DateTime)
               .WithColumn("response_xml").AsParaType(ParaTypes.LongText).Nullable()
               .WithColumn("response_date").AsParaType(ParaTypes.DateTime).Nullable()
               .WithColumn("status_code").AsParaType(ParaTypes.Int32).Nullable()
               .WithColumn("status_severity").AsParaType(ParaTypes.Text).Nullable()
               .WithColumn("status_message").AsParaType(ParaTypes.LongText).Nullable();

            Create.Index("uidx_paraquick_requests").OnTable("paraquick_requests")
                .OnColumn("ticket_id").Unique()
                .OnColumn("message_sequence").Unique()
                .OnColumn("request_sequence").Unique();


        }

        public static void ParaquickTables(this FluentMigrator.Builders.Delete.IDeleteExpressionRoot Delete)
        {
            Delete.Table("paraquick_requests");
            Delete.Table("paraquick_ticket_errors");
            Delete.Table("paraquick_tickets");
            Delete.Table("paraquick_ticket_statuses");
            Delete.Table("paraquick_estimates");
            Delete.Table("paraquick_customers");
            Delete.Table("paraquick_companies");

        }

    }
}
