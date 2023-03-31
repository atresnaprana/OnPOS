using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class changeidxstore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_xstore_organizations",
                table: "xstore_organization");

            migrationBuilder.AlterColumn<string>(
                name: "edp",
                table: "xstore_organization",
                type: "varchar(5)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(5)");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "xstore_organization",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_xstore_organizations_num",
                table: "xstore_organization",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_xstore_organizations_num",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "id",
                table: "xstore_organization");

            migrationBuilder.AlterColumn<string>(
                name: "edp",
                table: "xstore_organization",
                type: "varchar(5)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_xstore_organizations",
                table: "xstore_organization",
                column: "edp");
        }
    }
}
