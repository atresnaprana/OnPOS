using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addedsubtotal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "s41",
                table: "dbSalesHdr");

            migrationBuilder.DropColumn(
                name: "staff_id",
                table: "dbSalesHdr");

            migrationBuilder.AddColumn<int>(
                name: "finalprice",
                table: "dbSalesDtl",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "staff_id",
                table: "dbSalesDtl",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PKSaleshdr",
                table: "dbSalesHdr",
                columns: new[] { "Store_id", "invoice", "transdate" });

            migrationBuilder.AddPrimaryKey(
                name: "PKSalesdtl",
                table: "dbSalesDtl",
                columns: new[] { "store_id", "invoice", "transdate", "article" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PKSaleshdr",
                table: "dbSalesHdr");

            migrationBuilder.DropPrimaryKey(
                name: "PKSalesdtl",
                table: "dbSalesDtl");

            migrationBuilder.DropColumn(
                name: "finalprice",
                table: "dbSalesDtl");

            migrationBuilder.DropColumn(
                name: "staff_id",
                table: "dbSalesDtl");

            migrationBuilder.AddColumn<int>(
                name: "s41",
                table: "dbSalesHdr",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "staff_id",
                table: "dbSalesHdr",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
