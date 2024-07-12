using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addeddepartmenttbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "codedivisi",
                table: "dbItemMaster",
                type: "varchar(2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "dbDepartment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CodeDivisi = table.Column<string>(type: "varchar(2)", nullable: true),
                    DivisiName = table.Column<string>(type: "varchar(100)", nullable: true),
                    colorcode = table.Column<string>(type: "varchar(2)", nullable: true),
                    Color = table.Column<string>(type: "varchar(100)", nullable: true),
                    gendercode = table.Column<string>(type: "varchar(2)", nullable: true),
                    gender = table.Column<string>(type: "varchar(100)", nullable: true),
                    codemaincat = table.Column<string>(type: "varchar(2)", nullable: true),
                    maincat = table.Column<string>(type: "varchar(100)", nullable: true),
                    codesubcat = table.Column<string>(type: "varchar(2)", nullable: true),
                    subcat = table.Column<string>(type: "varchar(100)", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    ENTRY_USER = table.Column<string>(type: "varchar(50)", nullable: true),
                    UPDATE_USER = table.Column<string>(type: "varchar(10)", nullable: false),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbDepartment");

            migrationBuilder.DropColumn(
                name: "codedivisi",
                table: "dbItemMaster");
        }
    }
}
