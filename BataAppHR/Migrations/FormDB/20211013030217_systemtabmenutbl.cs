using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class systemtabmenutbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemMenuTbl",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TAB_ID = table.Column<int>(type: "int", nullable: false),
                    MENU_DESC = table.Column<string>(type: "varchar(5)", nullable: true),
                    ROLE_ID = table.Column<string>(type: "varchar(75)", nullable: true),
                    MENU_TXT = table.Column<string>(type: "varchar(50)", nullable: true),
                    MENU_LINK = table.Column<string>(nullable: true),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true),
                    ENTRY_USER = table.Column<string>(type: "varchar(50)", nullable: true),
                    UPDATE_USER = table.Column<string>(type: "varchar(10)", nullable: false),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MENU_Seq", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SystemTabTbl",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TAB_DESC = table.Column<string>(type: "varchar(5)", nullable: true),
                    ROLE_ID = table.Column<string>(type: "varchar(75)", nullable: true),
                    TAB_TXT = table.Column<string>(type: "varchar(50)", nullable: true),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true),
                    ENTRY_USER = table.Column<string>(type: "varchar(50)", nullable: true),
                    UPDATE_USER = table.Column<string>(type: "varchar(10)", nullable: false),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAB_Seq", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemMenuTbl");

            migrationBuilder.DropTable(
                name: "SystemTabTbl");
        }
    }
}
