using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addSalesWholeTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerTbl",
                table: "CustomerTbl");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SliderImg",
                table: "CustomerTbl",
                column: "id");

            migrationBuilder.CreateTable(
                name: "SalesWholeSaleTbl",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EMP_CODE = table.Column<string>(type: "varchar(5)", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    ENTRY_USER = table.Column<string>(type: "varchar(80)", nullable: true),
                    UPDATE_USER = table.Column<string>(type: "varchar(80)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SliderImg", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesWholeSaleTbl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SliderImg",
                table: "CustomerTbl");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerTbl",
                table: "CustomerTbl",
                column: "id");
        }
    }
}
