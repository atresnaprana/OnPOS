using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class updaterekaptraining : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalHours",
                table: "dbRekapTrainingFixed",
                type: "DECIMAL(6,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Hours",
                table: "dbRekapTrainingFixed",
                type: "DECIMAL(6,2)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Days",
                table: "dbRekapTrainingFixed",
                type: "DECIMAL(6,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TotalHours",
                table: "dbRekapTrainingFixed",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(6,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Hours",
                table: "dbRekapTrainingFixed",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(6,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Days",
                table: "dbRekapTrainingFixed",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(6,2)",
                oldNullable: true);
        }
    }
}
