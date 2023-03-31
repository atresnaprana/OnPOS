using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addedpickingno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "picking_no",
                table: "SalesOrderDtlCreditTbl",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "picking_no",
                table: "dbSalesOrder",
                type: "varchar(20)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "picking_no",
                table: "SalesOrderDtlCreditTbl");

            migrationBuilder.DropColumn(
                name: "picking_no",
                table: "dbSalesOrder");
        }
    }
}
