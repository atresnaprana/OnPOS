using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class updateflaggingdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FLAG_AKTIF",
                table: "scoreDB",
                type: "varchar(1)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FLAG_AKTIF",
                table: "dbTrainer",
                type: "varchar(1)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FLAG_AKTIF",
                table: "dbRekapTraining",
                type: "varchar(1)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FLAG_AKTIF",
                table: "scoreDB");

            migrationBuilder.DropColumn(
                name: "FLAG_AKTIF",
                table: "dbTrainer");

            migrationBuilder.DropColumn(
                name: "FLAG_AKTIF",
                table: "dbRekapTraining");
        }
    }
}
