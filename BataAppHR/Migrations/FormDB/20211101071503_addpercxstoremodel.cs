using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addpercxstoremodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DEPT_COMM_PERC",
                table: "xstore_organization",
                type: "decimal(11,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IS_PERC",
                table: "xstore_organization",
                type: "varchar(1)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RD_COMM_PERC",
                table: "xstore_organization",
                type: "decimal(11,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DEPT_COMM_PERC",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "IS_PERC",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "RD_COMM_PERC",
                table: "xstore_organization");
        }
    }
}
