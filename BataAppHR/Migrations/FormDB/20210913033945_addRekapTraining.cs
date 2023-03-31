using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addRekapTraining : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dbRekapTraining",
                columns: table => new
                {
                    TRN_ID = table.Column<string>(type: "varchar(50)", nullable: false),
                    Type = table.Column<string>(type: "varchar(255)", nullable: true),
                    Program = table.Column<string>(type: "varchar(255)", nullable: true),
                    ProgramTxt = table.Column<string>(type: "varchar(255)", nullable: true),
                    EDP = table.Column<string>(type: "varchar(255)", nullable: true),
                    Periode = table.Column<string>(type: "varchar(255)", nullable: true),
                    Week = table.Column<string>(type: "varchar(255)", nullable: true),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Participant = table.Column<string>(type: "varchar(255)", nullable: true),
                    Trainer = table.Column<string>(type: "varchar(255)", nullable: true),
                    idTrainer = table.Column<string>(type: "varchar(255)", nullable: true),
                    NoParticipant = table.Column<int>(type: "int", nullable: false),
                    Days = table.Column<int>(type: "int", nullable: false),
                    Hours = table.Column<int>(type: "int", nullable: false),
                    TotalHours = table.Column<int>(type: "int", nullable: false),
                    Entry_Date = table.Column<DateTime>(type: "date", nullable: false),
                    Entry_User = table.Column<string>(type: "varchar(100)", nullable: true),
                    Update_Date = table.Column<DateTime>(type: "date", nullable: false),
                    Update_User = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RekapTrainingTbl", x => x.TRN_ID);
                });

            migrationBuilder.CreateTable(
                name: "dbRekapTraining_seq",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RekapTraining_Seq", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbRekapTraining");

            migrationBuilder.DropTable(
                name: "dbRekapTraining_seq");
        }
    }
}
