using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addfilenamecust : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FILE_AKTA_NAME",
                table: "dbCustomer",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FILE_KTP_NAME",
                table: "dbCustomer",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FILE_NPWP_NAME",
                table: "dbCustomer",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FILE_REKENING_NAME",
                table: "dbCustomer",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FILE_SIUP_NAME",
                table: "dbCustomer",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FILE_TDP_NAME",
                table: "dbCustomer",
                type: "varchar(255)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FILE_AKTA_NAME",
                table: "dbCustomer");

            migrationBuilder.DropColumn(
                name: "FILE_KTP_NAME",
                table: "dbCustomer");

            migrationBuilder.DropColumn(
                name: "FILE_NPWP_NAME",
                table: "dbCustomer");

            migrationBuilder.DropColumn(
                name: "FILE_REKENING_NAME",
                table: "dbCustomer");

            migrationBuilder.DropColumn(
                name: "FILE_SIUP_NAME",
                table: "dbCustomer");

            migrationBuilder.DropColumn(
                name: "FILE_TDP_NAME",
                table: "dbCustomer");
        }
    }
}
