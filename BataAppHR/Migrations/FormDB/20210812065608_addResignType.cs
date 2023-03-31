using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addResignType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RESIGN_TYPE",
                table: "SSTable",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RESIGN_TYPE2",
                table: "SSTable",
                type: "varchar(255)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RESIGN_TYPE",
                table: "SSTable");

            migrationBuilder.DropColumn(
                name: "RESIGN_TYPE2",
                table: "SSTable");
        }
    }
}
