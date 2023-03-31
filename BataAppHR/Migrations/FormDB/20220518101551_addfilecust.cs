using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addfilecust : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "FILE_AKTA",
                table: "dbCustomer",
                type: "MediumBlob",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "FILE_KTP",
                table: "dbCustomer",
                type: "MediumBlob",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "FILE_NPWP",
                table: "dbCustomer",
                type: "MediumBlob",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "FILE_REKENING",
                table: "dbCustomer",
                type: "MediumBlob",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "FILE_SIUP",
                table: "dbCustomer",
                type: "MediumBlob",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "FILE_TDP",
                table: "dbCustomer",
                type: "MediumBlob",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FILE_AKTA",
                table: "dbCustomer");

            migrationBuilder.DropColumn(
                name: "FILE_KTP",
                table: "dbCustomer");

            migrationBuilder.DropColumn(
                name: "FILE_NPWP",
                table: "dbCustomer");

            migrationBuilder.DropColumn(
                name: "FILE_REKENING",
                table: "dbCustomer");

            migrationBuilder.DropColumn(
                name: "FILE_SIUP",
                table: "dbCustomer");

            migrationBuilder.DropColumn(
                name: "FILE_TDP",
                table: "dbCustomer");
        }
    }
}
