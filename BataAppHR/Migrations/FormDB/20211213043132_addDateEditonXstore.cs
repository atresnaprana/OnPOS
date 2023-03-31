using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addDateEditonXstore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ENTRY_DATE",
                table: "xstore_organization",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ENTRY_USER",
                table: "xstore_organization",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UPDATE_DATE",
                table: "xstore_organization",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UPDATE_USER",
                table: "xstore_organization",
                type: "varchar(255)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ENTRY_DATE",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "ENTRY_USER",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "UPDATE_DATE",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "UPDATE_USER",
                table: "xstore_organization");
        }
    }
}
