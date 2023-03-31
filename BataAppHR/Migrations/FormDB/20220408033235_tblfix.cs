using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class tblfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SliderImg",
                table: "SalesWholeSaleTbl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SliderImg",
                table: "CustomerTbl");

            migrationBuilder.RenameTable(
                name: "SalesWholeSaleTbl",
                newName: "dbSalesWholeSale");

            migrationBuilder.RenameTable(
                name: "CustomerTbl",
                newName: "dbCustomer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesWhole",
                table: "dbSalesWholeSale",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "dbCustomer",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesWhole",
                table: "dbSalesWholeSale");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "dbCustomer");

            migrationBuilder.RenameTable(
                name: "dbSalesWholeSale",
                newName: "SalesWholeSaleTbl");

            migrationBuilder.RenameTable(
                name: "dbCustomer",
                newName: "CustomerTbl");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SliderImg",
                table: "SalesWholeSaleTbl",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SliderImg",
                table: "CustomerTbl",
                column: "id");
        }
    }
}
