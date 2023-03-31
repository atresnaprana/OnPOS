using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class modifiedcustbasedata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KTP",
                table: "dbCustomer",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PHONE1",
                table: "dbCustomer",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PHONE2",
                table: "dbCustomer",
                type: "varchar(100)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KTP",
                table: "dbCustomer");

            migrationBuilder.DropColumn(
                name: "PHONE1",
                table: "dbCustomer");

            migrationBuilder.DropColumn(
                name: "PHONE2",
                table: "dbCustomer");
        }
    }
}
