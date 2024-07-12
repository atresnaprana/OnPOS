using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class movedstaffid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "staff_id",
                table: "dbSalesHdr");

            migrationBuilder.AddColumn<int>(
                name: "staff_id",
                table: "dbSalesDtl",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "staff_id",
                table: "dbSalesDtl");

            migrationBuilder.AddColumn<int>(
                name: "staff_id",
                table: "dbSalesHdr",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
