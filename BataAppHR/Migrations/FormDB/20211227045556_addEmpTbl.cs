using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addEmpTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dbEmployee",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EMP_CODE = table.Column<string>(type: "varchar(25)", nullable: true),
                    EMP_TYPE = table.Column<string>(type: "varchar(255)", nullable: true),
                    NM_EMP = table.Column<string>(type: "varchar(255)", nullable: true),
                    FLAG_AKTIF = table.Column<int>(type: "int", nullable: false),
                    RESIGN_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    RESIGN_TXT = table.Column<string>(type: "varchar(255)", nullable: true),
                    SEX = table.Column<string>(type: "varchar(255)", nullable: true),
                    EMP_HP = table.Column<string>(type: "varchar(255)", nullable: true),
                    EMP_EMAIL = table.Column<string>(type: "varchar(255)", nullable: true),
                    JOIN_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    EMP_SERAGAM_SIZE = table.Column<string>(type: "varchar(255)", nullable: true),
                    EMP_SEPATU_SIZEUK = table.Column<string>(type: "varchar(255)", nullable: true),
                    No_KTP = table.Column<string>(type: "varchar(255)", nullable: true),
                    VAKSIN1 = table.Column<string>(type: "varchar(255)", nullable: true),
                    VAKSIN2 = table.Column<string>(type: "varchar(255)", nullable: true),
                    FILE_SERTIFIKAT1 = table.Column<string>(type: "varchar(300)", nullable: true),
                    FILE_SERTIFIKAT2 = table.Column<string>(type: "varchar(300)", nullable: true),
                    ENTRY_USER = table.Column<string>(type: "varchar(255)", nullable: true),
                    UPDATE_USER = table.Column<string>(type: "varchar(255)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    EMP_PHOTO = table.Column<string>(type: "varchar(300)", nullable: true),
                    EMERGENCY_NAME = table.Column<string>(type: "varchar(255)", nullable: true),
                    EMERGENCY_PHONE = table.Column<string>(type: "varchar(255)", nullable: true),
                    EMERGENCY_ADDRESS = table.Column<string>(type: "varchar(300)", nullable: true),
                    RESIGN_TYPE = table.Column<string>(type: "varchar(255)", nullable: true),
                    RESIGN_TYPE2 = table.Column<string>(type: "varchar(255)", nullable: true),
                    FILE_MEDIC = table.Column<string>(type: "varchar(300)", nullable: true),
                    YEAR_LENGTH = table.Column<int>(type: "int", nullable: false),
                    MONTH_LENGTH = table.Column<int>(type: "int", nullable: false),
                    DAYS_LENGTH = table.Column<int>(type: "int", nullable: false),
                    LAMA_KERJA = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbEmp_Num", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbEmployee");
        }
    }
}
