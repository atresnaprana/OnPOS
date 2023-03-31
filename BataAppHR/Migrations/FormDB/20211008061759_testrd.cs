using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class testrd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RD",
                table: "dbRD");

            migrationBuilder.AlterColumn<string>(
                name: "RD_CODE",
                table: "dbRD",
                type: "varchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(10)");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "dbRD",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RD_num",
                table: "dbRD",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RD_num",
                table: "dbRD");

            migrationBuilder.DropColumn(
                name: "id",
                table: "dbRD");

            migrationBuilder.AlterColumn<string>(
                name: "RD_CODE",
                table: "dbRD",
                type: "varchar(10)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RD",
                table: "dbRD",
                column: "RD_CODE");
        }
    }
}
