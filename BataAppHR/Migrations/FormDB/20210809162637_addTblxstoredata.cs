using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addTblxstoredata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RESIGN_DATE",
                table: "SSTable",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "JOIN_DATE",
                table: "SSTable",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.CreateTable(
                name: "xstore_organization",
                columns: table => new
                {
                    edp = table.Column<string>(type: "varchar(5)", nullable: false),
                    district = table.Column<string>(type: "varchar(5)", nullable: true),
                    store_location = table.Column<string>(type: "varchar(75)", nullable: true),
                    area = table.Column<string>(type: "varchar(50)", nullable: true),
                    inactive_flag = table.Column<string>(type: "varchar(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_xstore_organizations", x => x.edp);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "xstore_organization");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RESIGN_DATE",
                table: "SSTable",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "JOIN_DATE",
                table: "SSTable",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);
        }
    }
}
