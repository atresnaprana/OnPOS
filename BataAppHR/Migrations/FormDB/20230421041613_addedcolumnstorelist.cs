using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addedcolumnstorelist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "STORE_EMAIL",
                table: "dbStoreList",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "STORE_MANAGER_EMAIL",
                table: "dbStoreList",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "STORE_MANAGER_NAME",
                table: "dbStoreList",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "STORE_MANAGER_PHONE",
                table: "dbStoreList",
                type: "varchar(255)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "STORE_EMAIL",
                table: "dbStoreList");

            migrationBuilder.DropColumn(
                name: "STORE_MANAGER_EMAIL",
                table: "dbStoreList");

            migrationBuilder.DropColumn(
                name: "STORE_MANAGER_NAME",
                table: "dbStoreList");

            migrationBuilder.DropColumn(
                name: "STORE_MANAGER_PHONE",
                table: "dbStoreList");
        }
    }
}
