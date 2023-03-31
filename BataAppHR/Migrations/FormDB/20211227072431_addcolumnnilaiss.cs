using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addcolumnnilaiss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EMP_TYPE",
                table: "dbNilaiSSFixed",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EMP_TYPE",
                table: "dbNilaiSS",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EMP_TYPE",
                table: "dbNilaiSSFixed");

            migrationBuilder.DropColumn(
                name: "EMP_TYPE",
                table: "dbNilaiSS");
        }
    }
}
