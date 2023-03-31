using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class revisedempreference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "id_sales",
                table: "dbSalesOrder");

            migrationBuilder.AddColumn<string>(
                name: "EMP_CODE",
                table: "dbSalesOrder",
                type: "varchar(25)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EMP_CODE",
                table: "dbSalesOrder");

            migrationBuilder.AddColumn<int>(
                name: "id_sales",
                table: "dbSalesOrder",
                type: "int(10)",
                nullable: false,
                defaultValue: 0);
        }
    }
}
