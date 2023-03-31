using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class updatetblsalesordercust : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "APPROVAL_AR",
                table: "dbSalesOrder",
                type: "varchar(1)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SHIPPING_ADDRESS",
                table: "dbSalesOrder",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "discount_customer",
                table: "dbCustomer",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "dbOrderConfig",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    pairsarticle = table.Column<int>(type: "int", nullable: false),
                    pairsorder = table.Column<int>(type: "int", nullable: false),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    ENTRY_USER = table.Column<string>(type: "varchar(50)", nullable: true),
                    UPDATE_USER = table.Column<string>(type: "varchar(10)", nullable: false),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderConfig", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbOrderConfig");

            migrationBuilder.DropColumn(
                name: "APPROVAL_AR",
                table: "dbSalesOrder");

            migrationBuilder.DropColumn(
                name: "SHIPPING_ADDRESS",
                table: "dbSalesOrder");

            migrationBuilder.DropColumn(
                name: "discount_customer",
                table: "dbCustomer");
        }
    }
}
