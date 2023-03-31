using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class modifyRDTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EMERGENCY_ADDRESS",
                table: "dbRD",
                type: "varchar(300)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EMERGENCY_NAME",
                table: "dbRD",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EMERGENCY_PHONE",
                table: "dbRD",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FILE_MEDIC",
                table: "dbRD",
                type: "varchar(300)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RD_PHOTO",
                table: "dbRD",
                type: "varchar(300)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RESIGN_TYPE",
                table: "dbRD",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RESIGN_TYPE2",
                table: "dbRD",
                type: "varchar(255)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EMERGENCY_ADDRESS",
                table: "dbRD");

            migrationBuilder.DropColumn(
                name: "EMERGENCY_NAME",
                table: "dbRD");

            migrationBuilder.DropColumn(
                name: "EMERGENCY_PHONE",
                table: "dbRD");

            migrationBuilder.DropColumn(
                name: "FILE_MEDIC",
                table: "dbRD");

            migrationBuilder.DropColumn(
                name: "RD_PHOTO",
                table: "dbRD");

            migrationBuilder.DropColumn(
                name: "RESIGN_TYPE",
                table: "dbRD");

            migrationBuilder.DropColumn(
                name: "RESIGN_TYPE2",
                table: "dbRD");
        }
    }
}
