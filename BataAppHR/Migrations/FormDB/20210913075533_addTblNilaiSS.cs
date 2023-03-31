using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addTblNilaiSS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "scoreDB",
                columns: table => new
                {
                    ScoreId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SS_CODE = table.Column<string>(type: "varchar(255)", nullable: true),
                    TRN_ID = table.Column<string>(type: "varchar(255)", nullable: true),
                    NILAI = table.Column<int>(type: "int", nullable: false),
                    SERTIFIKAT = table.Column<string>(type: "varchar(1)", nullable: false),
                    NoSertifikat = table.Column<string>(type: "varchar(255)", nullable: true),
                    FILE_SERTIFIKAT = table.Column<string>(type: "varchar(255)", nullable: true),
                    Entry_Date = table.Column<DateTime>(type: "date", nullable: false),
                    Entry_User = table.Column<string>(type: "varchar(100)", nullable: true),
                    Update_Date = table.Column<DateTime>(type: "date", nullable: false),
                    Update_User = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreTbl", x => x.ScoreId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "scoreDB");
        }
    }
}
