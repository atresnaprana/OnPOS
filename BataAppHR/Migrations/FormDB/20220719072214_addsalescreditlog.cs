using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addsalescreditlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dbSalesCreditLog",
                columns: table => new
                {
                    invno = table.Column<string>(type: "varchar(50)", nullable: true),
                    year = table.Column<int>(type: "int", nullable: false),
                    valuecredit = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    isused = table.Column<string>(type: "varchar(1)", nullable: true),
                    edp = table.Column<string>(type: "varchar(15)", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbSalesCreditLog");
        }
    }
}
