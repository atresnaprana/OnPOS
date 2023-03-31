using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addcolumnyearmonthday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DAYS_LENGTH",
                table: "SSTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MONTH_LENGTH",
                table: "SSTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YEAR_LENGTH",
                table: "SSTable",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DAYS_LENGTH",
                table: "SSTable");

            migrationBuilder.DropColumn(
                name: "MONTH_LENGTH",
                table: "SSTable");

            migrationBuilder.DropColumn(
                name: "YEAR_LENGTH",
                table: "SSTable");
        }
    }
}
