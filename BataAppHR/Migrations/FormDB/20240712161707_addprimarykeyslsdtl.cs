using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addprimarykeyslsdtl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PKSalesdtl",
                table: "dbSalesDtl");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "dbSalesDtl",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Salesdtl",
                table: "dbSalesDtl",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Salesdtl",
                table: "dbSalesDtl");

            migrationBuilder.DropColumn(
                name: "id",
                table: "dbSalesDtl");

            migrationBuilder.AddPrimaryKey(
                name: "PKSalesdtl",
                table: "dbSalesDtl",
                columns: new[] { "store_id", "invoice", "transdate", "article" });
        }
    }
}
