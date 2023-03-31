using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addedEntryUpdateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SS_CODE",
                table: "SSTable",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AddColumn<DateTime>(
                name: "ENTRY_DATE",
                table: "SSTable",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ENTRY_USER",
                table: "SSTable",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UPDATE_DATE",
                table: "SSTable",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UPDATE_USER",
                table: "SSTable",
                type: "varchar(255)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ENTRY_DATE",
                table: "SSTable");

            migrationBuilder.DropColumn(
                name: "ENTRY_USER",
                table: "SSTable");

            migrationBuilder.DropColumn(
                name: "UPDATE_DATE",
                table: "SSTable");

            migrationBuilder.DropColumn(
                name: "UPDATE_USER",
                table: "SSTable");

            migrationBuilder.AlterColumn<string>(
                name: "SS_CODE",
                table: "SSTable",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);
        }
    }
}
