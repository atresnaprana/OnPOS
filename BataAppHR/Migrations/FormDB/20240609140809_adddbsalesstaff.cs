using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class adddbsalesstaff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dbSalesStaff",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    COMPANY_ID = table.Column<string>(type: "varchar(50)", nullable: true),
                    STORE_ID = table.Column<int>(type: "int", nullable: false),
                    SALES_NAME = table.Column<string>(type: "varchar(255)", nullable: true),
                    SALES_REG_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    ENTRY_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    ENTRY_USER = table.Column<string>(type: "varchar(255)", nullable: true),
                    UPDATE_USER = table.Column<string>(type: "varchar(255)", nullable: true),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true),
                    FILE_PHOTO = table.Column<byte[]>(type: "MediumBlob", nullable: true),
                    FILE_PHOTO_NAME = table.Column<string>(type: "varchar(255)", nullable: true),
                    SALES_PHONE = table.Column<string>(type: "varchar(255)", nullable: true),
                    SALES_EMAIL = table.Column<string>(type: "varchar(255)", nullable: true),
                    SALES_KTP = table.Column<string>(type: "varchar(255)", nullable: true),
                    SALES_BLACKLIST_FLAG = table.Column<string>(type: "varchar(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesStaff", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbSalesStaff");
        }
    }
}
