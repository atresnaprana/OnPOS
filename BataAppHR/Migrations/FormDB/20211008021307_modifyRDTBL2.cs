using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class modifyRDTBL2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DAYS_LENGTH",
                table: "dbRD",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LAMA_KERJA",
                table: "dbRD",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MONTH_LENGTH",
                table: "dbRD",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YEAR_LENGTH",
                table: "dbRD",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DAYS_LENGTH",
                table: "dbRD");

            migrationBuilder.DropColumn(
                name: "LAMA_KERJA",
                table: "dbRD");

            migrationBuilder.DropColumn(
                name: "MONTH_LENGTH",
                table: "dbRD");

            migrationBuilder.DropColumn(
                name: "YEAR_LENGTH",
                table: "dbRD");
        }
    }
}
