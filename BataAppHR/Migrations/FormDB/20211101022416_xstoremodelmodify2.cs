using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class xstoremodelmodify2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CLOSE_DATE",
                table: "xstore_organization",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DEPT_COMM",
                table: "xstore_organization",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "IS_DS",
                table: "xstore_organization",
                type: "varchar(1)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RD_COMM",
                table: "xstore_organization",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CLOSE_DATE",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "DEPT_COMM",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "IS_DS",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "RD_COMM",
                table: "xstore_organization");
        }
    }
}
