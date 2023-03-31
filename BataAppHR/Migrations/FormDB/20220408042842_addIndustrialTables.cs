using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addIndustrialTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EMP_CODE",
                table: "dbSalesWholeSale",
                type: "varchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "dbPaymentList",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_order = table.Column<int>(type: "int(10)", nullable: false),
                    id_customer = table.Column<int>(type: "int(10)", nullable: false),
                    BANK = table.Column<string>(type: "varchar(255)", nullable: true),
                    REF_ID = table.Column<string>(type: "varchar(255)", nullable: true),
                    FILE_PAYMENT = table.Column<byte[]>(type: "MediumBlob", nullable: true),
                    TOTAL_PAY = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    ENTRY_USER = table.Column<string>(type: "varchar(80)", nullable: true),
                    UPDATE_USER = table.Column<string>(type: "varchar(80)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentList", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbSalesOrder",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_sales = table.Column<int>(type: "int(10)", nullable: false),
                    TOTAL_ORDER = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    TOTAL_QTY = table.Column<int>(type: "int(10)", nullable: false),
                    STATUS = table.Column<string>(type: "varchar(10)", nullable: true),
                    APPROVAL_1 = table.Column<string>(type: "varchar(10)", nullable: true),
                    APPROVAL_2 = table.Column<string>(type: "varchar(10)", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    ENTRY_USER = table.Column<string>(type: "varchar(80)", nullable: true),
                    UPDATE_USER = table.Column<string>(type: "varchar(80)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrder", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbSalesOrderDtl",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_order = table.Column<int>(type: "int(10)", nullable: false),
                    article = table.Column<string>(type: "varchar(10)", nullable: true),
                    size = table.Column<string>(type: "varchar(10)", nullable: true),
                    qty = table.Column<int>(type: "int(10)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    disc = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    final_price = table.Column<decimal>(type: "decimal(15,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderDtl", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbPaymentList");

            migrationBuilder.DropTable(
                name: "dbSalesOrder");

            migrationBuilder.DropTable(
                name: "dbSalesOrderDtl");

            migrationBuilder.AlterColumn<string>(
                name: "EMP_CODE",
                table: "dbSalesWholeSale",
                type: "varchar(5)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldNullable: true);
        }
    }
}
