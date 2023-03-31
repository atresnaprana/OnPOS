using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addcolumndata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EMERGENCY_ADDRESS",
                table: "SSTable",
                type: "varchar(300)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EMERGENCY_NAME",
                table: "SSTable",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EMERGENCY_PHONE",
                table: "SSTable",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "STAFF_PHOTO",
                table: "SSTable",
                type: "varchar(300)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EMERGENCY_ADDRESS",
                table: "SSTable");

            migrationBuilder.DropColumn(
                name: "EMERGENCY_NAME",
                table: "SSTable");

            migrationBuilder.DropColumn(
                name: "EMERGENCY_PHONE",
                table: "SSTable");

            migrationBuilder.DropColumn(
                name: "STAFF_PHOTO",
                table: "SSTable");
        }
    }
}
