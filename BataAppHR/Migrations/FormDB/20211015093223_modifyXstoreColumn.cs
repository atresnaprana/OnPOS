using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class modifyXstoreColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ADDRESS",
                table: "xstore_organization",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FILE_LEASE",
                table: "xstore_organization",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FILE_OTHERS",
                table: "xstore_organization",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GROSS_VAL",
                table: "xstore_organization",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_RENOVATION_DATE",
                table: "xstore_organization",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LEASE_EXPIRED",
                table: "xstore_organization",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LEASE_START",
                table: "xstore_organization",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OPENING_DATE",
                table: "xstore_organization",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SELLING_AREA",
                table: "xstore_organization",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SELLING_VAL",
                table: "xstore_organization",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "STOCK_AREA",
                table: "xstore_organization",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "STORAGE_VAL",
                table: "xstore_organization",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "STORE_CONCEPT",
                table: "xstore_organization",
                type: "varchar(150)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "STORE_IMAGE",
                table: "xstore_organization",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TOTAL_AREA",
                table: "xstore_organization",
                type: "decimal(5,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ADDRESS",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "FILE_LEASE",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "FILE_OTHERS",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "GROSS_VAL",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "LAST_RENOVATION_DATE",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "LEASE_EXPIRED",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "LEASE_START",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "OPENING_DATE",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "SELLING_AREA",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "SELLING_VAL",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "STOCK_AREA",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "STORAGE_VAL",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "STORE_CONCEPT",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "STORE_IMAGE",
                table: "xstore_organization");

            migrationBuilder.DropColumn(
                name: "TOTAL_AREA",
                table: "xstore_organization");
        }
    }
}
