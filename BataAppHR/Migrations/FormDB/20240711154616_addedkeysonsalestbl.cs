using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addedkeysonsalestbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "size",
                table: "dbSalesDtl",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PKSaleshdr",
                table: "dbSalesHdr",
                columns: new[] { "Store_id", "invoice", "transdate" });

            migrationBuilder.AddPrimaryKey(
                name: "PKSalesdtl",
                table: "dbSalesDtl",
                columns: new[] { "store_id", "invoice", "transdate", "article", "size" });
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
                name: "size",
                table: "dbSalesDtl");
        }
    }
}
