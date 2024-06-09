using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class removesstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SSTable");

            migrationBuilder.DropTable(
                name: "sstable_seq");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dbSalesCreditLog",
                table: "dbSalesCreditLog");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "dbSubCategory",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "brand",
                table: "dbItemMaster",
                type: "varchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Creditlogs",
                table: "dbSalesCreditLog",
                column: "edp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Creditlogs",
                table: "dbSalesCreditLog");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "dbSubCategory",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "brand",
                table: "dbItemMaster",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_dbSalesCreditLog",
                table: "dbSalesCreditLog",
                columns: new[] { "edp", "invno" });

            migrationBuilder.CreateTable(
                name: "SSTable",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DAYS_LENGTH = table.Column<int>(type: "int", nullable: false),
                    EDP_CODE = table.Column<string>(type: "varchar(5)", nullable: false),
                    EMAIL_SS = table.Column<string>(type: "varchar(255)", nullable: true),
                    EMERGENCY_ADDRESS = table.Column<string>(type: "varchar(300)", nullable: true),
                    EMERGENCY_NAME = table.Column<string>(type: "varchar(255)", nullable: true),
                    EMERGENCY_PHONE = table.Column<string>(type: "varchar(255)", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    ENTRY_USER = table.Column<string>(type: "varchar(255)", nullable: true),
                    FILE_MEDIC = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FLAG_AKTIF = table.Column<int>(type: "int", nullable: false),
                    FOTOSERTIFIKAT1 = table.Column<string>(type: "varchar(300)", nullable: true),
                    FOTOSERTIFIKAT2 = table.Column<string>(type: "varchar(300)", nullable: true),
                    HP_SS = table.Column<string>(type: "varchar(255)", nullable: true),
                    JOIN_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    KTP = table.Column<string>(type: "varchar(255)", nullable: true),
                    LAMA_KERJA = table.Column<string>(type: "varchar(255)", nullable: true),
                    MONTH_LENGTH = table.Column<int>(type: "int", nullable: false),
                    NAMA_SS = table.Column<string>(type: "varchar(255)", nullable: false),
                    POSITION = table.Column<string>(type: "varchar(255)", nullable: true),
                    RESIGN_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    RESIGN_TXT = table.Column<string>(type: "varchar(255)", nullable: true),
                    RESIGN_TYPE = table.Column<string>(type: "varchar(255)", nullable: true),
                    RESIGN_TYPE2 = table.Column<string>(type: "varchar(255)", nullable: true),
                    SEX = table.Column<string>(type: "varchar(255)", nullable: true),
                    SIZE_SEPATU_UK = table.Column<string>(type: "varchar(255)", nullable: true),
                    SIZE_SERAGAM = table.Column<string>(type: "varchar(255)", nullable: true),
                    SS_CODE = table.Column<string>(type: "varchar(255)", nullable: true),
                    STAFF_PHOTO = table.Column<string>(type: "varchar(300)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    UPDATE_USER = table.Column<string>(type: "varchar(255)", nullable: true),
                    VAKSIN1 = table.Column<string>(type: "varchar(255)", nullable: true),
                    VAKSIN2 = table.Column<string>(type: "varchar(255)", nullable: true),
                    XSTORE_LOGIN = table.Column<string>(type: "varchar(255)", nullable: true),
                    YEAR_LENGTH = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SSTable", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "sstable_seq",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SSTable_Seq", x => x.id);
                });
        }
    }
}
