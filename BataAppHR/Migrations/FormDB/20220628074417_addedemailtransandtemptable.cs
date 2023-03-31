using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addedemailtransandtemptable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EMAIL",
                table: "dbSalesOrder",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "dbSalesOrderDtlTemp",
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
                    is_disc_perc = table.Column<string>(type: "varchar(10)", nullable: true),
                    disc_amt = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    final_price = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    Size_1 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_2 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_3 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_4 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_5 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_6 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_7 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_8 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_9 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_10 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_11 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_12 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_13 = table.Column<int>(type: "int(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderDtlTemp", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbSalesOrderTemp",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_customer = table.Column<int>(type: "int(10)", nullable: true),
                    EMP_CODE = table.Column<string>(type: "varchar(25)", nullable: true),
                    TOTAL_ORDER = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    TOTAL_QTY = table.Column<int>(type: "int(10)", nullable: false),
                    STATUS = table.Column<string>(type: "varchar(10)", nullable: true),
                    APPROVAL_1 = table.Column<string>(type: "varchar(10)", nullable: true),
                    APPROVAL_2 = table.Column<string>(type: "varchar(10)", nullable: true),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true),
                    ORDER_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    ENTRY_USER = table.Column<string>(type: "varchar(80)", nullable: true),
                    UPDATE_USER = table.Column<string>(type: "varchar(80)", nullable: true),
                    EMAIL = table.Column<string>(type: "varchar(255)", nullable: true),
                    INV_DISC = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    FLAG_SENT = table.Column<string>(type: "varchar(10)", nullable: true),
                    IS_DISC_PERC = table.Column<string>(type: "varchar(10)", nullable: true),
                    INV_DISC_AMT = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    SHIPPING_ADDRESS = table.Column<string>(type: "varchar(255)", nullable: true),
                    APPROVAL_AR = table.Column<string>(type: "varchar(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderTemp", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbSalesOrderDtlTemp");

            migrationBuilder.DropTable(
                name: "dbSalesOrderTemp");

            migrationBuilder.DropColumn(
                name: "EMAIL",
                table: "dbSalesOrder");
        }
    }
}
