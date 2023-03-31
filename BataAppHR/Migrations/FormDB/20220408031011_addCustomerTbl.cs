using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addCustomerTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerTbl",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EDP = table.Column<string>(type: "varchar(5)", nullable: true),
                    CUST_NAME = table.Column<string>(type: "varchar(255)", nullable: true),
                    COMPANY = table.Column<string>(type: "varchar(255)", nullable: true),
                    NPWP = table.Column<string>(type: "varchar(25)", nullable: true),
                    address = table.Column<string>(type: "varchar(255)", nullable: true),
                    city = table.Column<string>(type: "varchar(80)", nullable: true),
                    province = table.Column<string>(type: "varchar(80)", nullable: true),
                    postal = table.Column<string>(type: "varchar(10)", nullable: true),
                    BANK_NAME = table.Column<string>(type: "varchar(255)", nullable: true),
                    BANK_NUMBER = table.Column<string>(type: "varchar(255)", nullable: true),
                    BANK_BRANCH = table.Column<string>(type: "varchar(255)", nullable: true),
                    BANK_COUNTRY = table.Column<string>(type: "varchar(255)", nullable: true),
                    REG_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    BL_FLAG = table.Column<string>(type: "varchar(1)", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    ENTRY_USER = table.Column<string>(type: "varchar(80)", nullable: true),
                    UPDATE_USER = table.Column<string>(type: "varchar(80)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTbl", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerTbl");
        }
    }
}
