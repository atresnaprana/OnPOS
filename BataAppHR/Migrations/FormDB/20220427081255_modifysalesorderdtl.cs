using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class modifysalesorderdtl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Size_1",
                table: "dbSalesOrderDtl",
                type: "int(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size_10",
                table: "dbSalesOrderDtl",
                type: "int(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size_11",
                table: "dbSalesOrderDtl",
                type: "int(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size_12",
                table: "dbSalesOrderDtl",
                type: "int(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size_13",
                table: "dbSalesOrderDtl",
                type: "int(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size_2",
                table: "dbSalesOrderDtl",
                type: "int(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size_3",
                table: "dbSalesOrderDtl",
                type: "int(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size_4",
                table: "dbSalesOrderDtl",
                type: "int(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size_5",
                table: "dbSalesOrderDtl",
                type: "int(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size_6",
                table: "dbSalesOrderDtl",
                type: "int(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size_7",
                table: "dbSalesOrderDtl",
                type: "int(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size_8",
                table: "dbSalesOrderDtl",
                type: "int(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size_9",
                table: "dbSalesOrderDtl",
                type: "int(10)",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size_1",
                table: "dbSalesOrderDtl");

            migrationBuilder.DropColumn(
                name: "Size_10",
                table: "dbSalesOrderDtl");

            migrationBuilder.DropColumn(
                name: "Size_11",
                table: "dbSalesOrderDtl");

            migrationBuilder.DropColumn(
                name: "Size_12",
                table: "dbSalesOrderDtl");

            migrationBuilder.DropColumn(
                name: "Size_13",
                table: "dbSalesOrderDtl");

            migrationBuilder.DropColumn(
                name: "Size_2",
                table: "dbSalesOrderDtl");

            migrationBuilder.DropColumn(
                name: "Size_3",
                table: "dbSalesOrderDtl");

            migrationBuilder.DropColumn(
                name: "Size_4",
                table: "dbSalesOrderDtl");

            migrationBuilder.DropColumn(
                name: "Size_5",
                table: "dbSalesOrderDtl");

            migrationBuilder.DropColumn(
                name: "Size_6",
                table: "dbSalesOrderDtl");

            migrationBuilder.DropColumn(
                name: "Size_7",
                table: "dbSalesOrderDtl");

            migrationBuilder.DropColumn(
                name: "Size_8",
                table: "dbSalesOrderDtl");

            migrationBuilder.DropColumn(
                name: "Size_9",
                table: "dbSalesOrderDtl");
        }
    }
}
