using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addedvanumcust : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VA1",
                table: "dbCustomer",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VA1NOTE",
                table: "dbCustomer",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VA2",
                table: "dbCustomer",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VA2NOTE",
                table: "dbCustomer",
                type: "varchar(100)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VA1",
                table: "dbCustomer");

            migrationBuilder.DropColumn(
                name: "VA1NOTE",
                table: "dbCustomer");

            migrationBuilder.DropColumn(
                name: "VA2",
                table: "dbCustomer");

            migrationBuilder.DropColumn(
                name: "VA2NOTE",
                table: "dbCustomer");
        }
    }
}
