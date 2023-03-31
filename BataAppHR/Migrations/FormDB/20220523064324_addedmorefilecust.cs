using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addedmorefilecust : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "FILE_NIB",
                table: "dbCustomer",
                type: "MediumBlob",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FILE_NIB_NAME",
                table: "dbCustomer",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "FILE_SKT",
                table: "dbCustomer",
                type: "MediumBlob",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FILE_SKT_NAME",
                table: "dbCustomer",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "FILE_SPPKP",
                table: "dbCustomer",
                type: "MediumBlob",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FILE_SPPKP_NAME",
                table: "dbCustomer",
                type: "varchar(255)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FILE_NIB",
                table: "dbCustomer");

            migrationBuilder.DropColumn(
                name: "FILE_NIB_NAME",
                table: "dbCustomer");

            migrationBuilder.DropColumn(
                name: "FILE_SKT",
                table: "dbCustomer");

            migrationBuilder.DropColumn(
                name: "FILE_SKT_NAME",
                table: "dbCustomer");

            migrationBuilder.DropColumn(
                name: "FILE_SPPKP",
                table: "dbCustomer");

            migrationBuilder.DropColumn(
                name: "FILE_SPPKP_NAME",
                table: "dbCustomer");
        }
    }
}
