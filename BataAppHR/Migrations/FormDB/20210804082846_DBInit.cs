using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class DBInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SSTable",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SS_CODE = table.Column<string>(type: "varchar(255)", nullable: false),
                    NAMA_SS = table.Column<string>(type: "varchar(255)", nullable: true),
                    FLAG_AKTIF = table.Column<int>(type: "int", nullable: false),
                    SEX = table.Column<string>(type: "varchar(255)", nullable: true),
                    KTP = table.Column<string>(type: "varchar(255)", nullable: true),
                    HP_SS = table.Column<string>(type: "varchar(255)", nullable: true),
                    EMAIL_SS = table.Column<string>(type: "varchar(255)", nullable: true),
                    SIZE_SERAGAM = table.Column<string>(type: "varchar(255)", nullable: true),
                    SIZE_SEPATU_UK = table.Column<string>(type: "varchar(255)", nullable: true),
                    RESIGN_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    JOIN_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    RESIGN_TXT = table.Column<string>(type: "varchar(255)", nullable: true),
                    LAMA_KERJA = table.Column<string>(type: "varchar(255)", nullable: true),
                    VAKSIN1 = table.Column<string>(type: "varchar(255)", nullable: true),
                    VAKSIN2 = table.Column<string>(type: "varchar(255)", nullable: true),
                    FOTOSERTIFIKAT1 = table.Column<string>(type: "varchar(300)", nullable: true),
                    FOTOSERTIFIKAT2 = table.Column<string>(type: "varchar(300)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SSTable", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SSTable");
        }
    }
}
