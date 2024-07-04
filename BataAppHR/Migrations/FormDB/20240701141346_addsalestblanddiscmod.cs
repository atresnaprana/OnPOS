using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addsalestblanddiscmod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "dbDiscount",
                type: "varchar(1)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "validfrom",
                table: "dbDiscount",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "validto",
                table: "dbDiscount",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "dbSalesDtl",
                columns: table => new
                {
                    store_id = table.Column<int>(type: "int", nullable: false),
                    invoice = table.Column<string>(type: "varchar(50)", nullable: false),
                    transdate = table.Column<DateTime>(type: "datetime", nullable: false),
                    article = table.Column<string>(type: "varchar(50)", nullable: false),
                    cat = table.Column<string>(type: "varchar(50)", nullable: true),
                    subcat = table.Column<string>(type: "varchar(50)", nullable: true),
                    price = table.Column<int>(type: "int", nullable: false),
                    discountcode = table.Column<int>(type: "int", nullable: false),
                    disc_amount = table.Column<int>(type: "int", nullable: false),
                    disc_prc = table.Column<int>(type: "int", nullable: false),
                    qty = table.Column<int>(type: "int", nullable: false),
                    s33 = table.Column<int>(type: "int", nullable: false),
                    s34 = table.Column<int>(type: "int", nullable: false),
                    s35 = table.Column<int>(type: "int", nullable: false),
                    s36 = table.Column<int>(type: "int", nullable: false),
                    s37 = table.Column<int>(type: "int", nullable: false),
                    s38 = table.Column<int>(type: "int", nullable: false),
                    s39 = table.Column<int>(type: "int", nullable: false),
                    s40 = table.Column<int>(type: "int", nullable: false),
                    s41 = table.Column<int>(type: "int", nullable: false),
                    s42 = table.Column<int>(type: "int", nullable: false),
                    s43 = table.Column<int>(type: "int", nullable: false),
                    s44 = table.Column<int>(type: "int", nullable: false),
                    s45 = table.Column<int>(type: "int", nullable: false),
                    s46 = table.Column<int>(type: "int", nullable: false),
                    update_user = table.Column<string>(type: "varchar(255)", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "dbSalesHdr",
                columns: table => new
                {
                    invoice = table.Column<string>(type: "varchar(50)", nullable: false),
                    Store_id = table.Column<int>(type: "int", nullable: false),
                    staff_id = table.Column<int>(type: "int", nullable: false),
                    transdate = table.Column<DateTime>(type: "datetime", nullable: false),
                    trans_amount = table.Column<int>(type: "int", nullable: false),
                    trans_qty = table.Column<int>(type: "int", nullable: false),
                    approval_code = table.Column<string>(type: "varchar(100)", nullable: true),
                    cardnum = table.Column<string>(type: "varchar(100)", nullable: true),
                    transtype = table.Column<string>(type: "varchar(100)", nullable: true),
                    update_user = table.Column<string>(type: "varchar(255)", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    s41 = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbSalesDtl");

            migrationBuilder.DropTable(
                name: "dbSalesHdr");

            migrationBuilder.DropColumn(
                name: "status",
                table: "dbDiscount");

            migrationBuilder.DropColumn(
                name: "validfrom",
                table: "dbDiscount");

            migrationBuilder.DropColumn(
                name: "validto",
                table: "dbDiscount");
        }
    }
}
