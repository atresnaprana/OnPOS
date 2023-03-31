using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class modifyidcust : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "id_customer",
                table: "dbSalesOrder",
                type: "int(10)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int(10)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "id_customer",
                table: "dbSalesOrder",
                type: "int(10)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int(10)",
                oldNullable: true);
        }
    }
}
