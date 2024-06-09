using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addingitemmaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dbCategory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    COMPANY_ID = table.Column<string>(type: "varchar(50)", nullable: true),
                    Category = table.Column<string>(type: "varchar(50)", nullable: true),
                    description = table.Column<string>(type: "varchar(100)", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    ENTRY_USER = table.Column<string>(type: "varchar(50)", nullable: true),
                    UPDATE_USER = table.Column<string>(type: "varchar(10)", nullable: false),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbItemMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    COMPANY_ID = table.Column<string>(type: "varchar(50)", nullable: true),
                    itemid = table.Column<string>(type: "varchar(50)", nullable: true),
                    color = table.Column<string>(type: "varchar(50)", nullable: true),
                    size = table.Column<string>(type: "varchar(50)", nullable: true),
                    category = table.Column<string>(type: "varchar(50)", nullable: true),
                    subcategory = table.Column<string>(type: "varchar(50)", nullable: true),
                    itemdescription = table.Column<string>(type: "varchar(255)", nullable: true),
                    price1 = table.Column<decimal>(type: "decimal(19,2)", nullable: false),
                    price2 = table.Column<decimal>(type: "decimal(19,2)", nullable: false),
                    price3 = table.Column<decimal>(type: "decimal(19,2)", nullable: false),
                    brand = table.Column<string>(type: "varchar(100)", nullable: false),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    ENTRY_USER = table.Column<string>(type: "varchar(50)", nullable: true),
                    UPDATE_USER = table.Column<string>(type: "varchar(10)", nullable: false),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemMaster", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbItemStore",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    COMPANY_ID = table.Column<string>(type: "varchar(50)", nullable: true),
                    itemidint = table.Column<int>(type: "int", nullable: false),
                    itemid = table.Column<string>(type: "varchar(50)", nullable: true),
                    storeid = table.Column<int>(type: "int", nullable: false),
                    STORE_NAME = table.Column<string>(type: "varchar(50)", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    ENTRY_USER = table.Column<string>(type: "varchar(50)", nullable: true),
                    UPDATE_USER = table.Column<string>(type: "varchar(10)", nullable: false),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemStore", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbSubCategory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    COMPANY_ID = table.Column<string>(type: "varchar(50)", nullable: true),
                    Category = table.Column<string>(type: "varchar(50)", nullable: true),
                    SubCategory = table.Column<string>(type: "varchar(50)", nullable: true),
                    description = table.Column<string>(type: "varchar(100)", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    ENTRY_USER = table.Column<string>(type: "varchar(50)", nullable: true),
                    UPDATE_USER = table.Column<string>(type: "varchar(10)", nullable: false),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategory", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbCategory");

            migrationBuilder.DropTable(
                name: "dbItemMaster");

            migrationBuilder.DropTable(
                name: "dbItemStore");

            migrationBuilder.DropTable(
                name: "dbSubCategory");
        }
    }
}
