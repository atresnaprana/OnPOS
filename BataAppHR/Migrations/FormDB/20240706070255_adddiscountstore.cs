using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class adddiscountstore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "storeid",
                table: "dbDiscount");

            migrationBuilder.CreateTable(
                name: "dbDiscountStoreList",
                columns: table => new
                {
                    COMPANY_ID = table.Column<string>(type: "varchar(100)", nullable: true),
                    storeid = table.Column<int>(type: "int", nullable: false),
                    promoid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbDiscountStoreList");

            migrationBuilder.AddColumn<int>(
                name: "storeid",
                table: "dbDiscount",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
