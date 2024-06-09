using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addstorelisttbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "COMPANY_ID",
                table: "dbCustomer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "totalstoreconfig",
                table: "dbCustomer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "dbCompany_seq",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company_Seq", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbStoreList",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    COMPANY_ID = table.Column<string>(type: "varchar(50)", nullable: true),
                    STORE_NAME = table.Column<string>(type: "varchar(255)", nullable: true),
                    STORE_ADDRESS = table.Column<string>(type: "varchar(255)", nullable: true),
                    STORE_CITY = table.Column<string>(type: "varchar(255)", nullable: true),
                    STORE_PROVINCE = table.Column<string>(type: "varchar(255)", nullable: true),
                    STORE_POSTAL = table.Column<string>(type: "varchar(255)", nullable: true),
                    STORE_BANK_NAME = table.Column<string>(type: "varchar(255)", nullable: true),
                    STORE_BANK_NUMBER = table.Column<string>(type: "varchar(255)", nullable: true),
                    STORE_BANK_BRANCH = table.Column<string>(type: "varchar(255)", nullable: true),
                    STORE_BANK_COUNTRY = table.Column<string>(type: "varchar(255)", nullable: true),
                    STORE_REG_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    STORE_BL_FLAG = table.Column<string>(type: "varchar(1)", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    ENTRY_USER = table.Column<string>(type: "varchar(255)", nullable: true),
                    UPDATE_USER = table.Column<string>(type: "varchar(255)", nullable: true),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true),
                    FILE_PHOTO = table.Column<byte[]>(type: "MediumBlob", nullable: true),
                    FILE_PHOTO_NAME = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreList", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbCompany_seq");

            migrationBuilder.DropTable(
                name: "dbStoreList");

            migrationBuilder.DropColumn(
                name: "COMPANY_ID",
                table: "dbCustomer");

            migrationBuilder.DropColumn(
                name: "totalstoreconfig",
                table: "dbCustomer");
        }
    }
}
