using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addtblRD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dbRD",
                columns: table => new
                {
                    RD_CODE = table.Column<string>(type: "varchar(10)", nullable: false),
                    EDP_CODE = table.Column<string>(type: "varchar(5)", nullable: true),
                    NM_RD = table.Column<string>(type: "varchar(255)", nullable: true),
                    FLAG_AKTIF = table.Column<int>(type: "int", nullable: false),
                    RESIGN_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    RESIGN_TXT = table.Column<string>(type: "varchar(255)", nullable: true),
                    SEX = table.Column<string>(type: "varchar(255)", nullable: true),
                    RD_HP = table.Column<string>(type: "varchar(255)", nullable: true),
                    RD_EMAIL = table.Column<string>(type: "varchar(255)", nullable: true),
                    JOIN_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    RD_SERAGAM_SIZE = table.Column<string>(type: "varchar(255)", nullable: true),
                    RD_SEPATU_SIZEUK = table.Column<string>(type: "varchar(255)", nullable: true),
                    No_KTP = table.Column<string>(type: "varchar(255)", nullable: true),
                    VAKSIN1 = table.Column<string>(type: "varchar(255)", nullable: true),
                    VAKSIN2 = table.Column<string>(type: "varchar(255)", nullable: true),
                    FILE_SERTIFIKAT1 = table.Column<string>(type: "varchar(300)", nullable: true),
                    FILE_SERTIFIKAT2 = table.Column<string>(type: "varchar(300)", nullable: true),
                    ENTRY_USER = table.Column<string>(type: "varchar(255)", nullable: true),
                    UPDATE_USER = table.Column<string>(type: "varchar(255)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RD", x => x.RD_CODE);
                });

            migrationBuilder.CreateTable(
                name: "dbRDSeq",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RD_Seq", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbRD");

            migrationBuilder.DropTable(
                name: "dbRDSeq");
        }
    }
}
