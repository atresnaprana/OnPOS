using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addstocktbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CY_amount",
                table: "dbItemMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CY_qty",
                table: "dbItemMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LY_amount",
                table: "dbItemMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LY_qty",
                table: "dbItemMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "month_age",
                table: "dbItemMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "month_amount",
                table: "dbItemMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "month_qty",
                table: "dbItemMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "year_age",
                table: "dbItemMaster",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "dbStoreStock",
                columns: table => new
                {
                    stock_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    storeid = table.Column<int>(type: "int", nullable: false),
                    itemid = table.Column<string>(type: "varchar(50)", nullable: true),
                    itmname = table.Column<string>(type: "varchar(255)", nullable: true),
                    cat = table.Column<string>(type: "varchar(50)", nullable: true),
                    subcat = table.Column<string>(type: "varchar(50)", nullable: true),
                    area = table.Column<string>(type: "varchar(50)", nullable: true),
                    row = table.Column<string>(type: "varchar(50)", nullable: true),
                    rack = table.Column<string>(type: "varchar(50)", nullable: true),
                    racklvl = table.Column<string>(type: "varchar(50)", nullable: true),
                    bin_id = table.Column<string>(type: "varchar(50)", nullable: true),
                    Past_stock = table.Column<int>(type: "int", nullable: false),
                    dispatch_qty = table.Column<int>(type: "int", nullable: false),
                    sales_qty = table.Column<int>(type: "int", nullable: false),
                    receive_qty = table.Column<int>(type: "int", nullable: false),
                    Current_stock = table.Column<int>(type: "int", nullable: false),
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
                    standard_qty = table.Column<int>(type: "int", nullable: false),
                    lastrcvdate = table.Column<DateTime>(type: "datetime", nullable: true),
                    lastoutdate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbStoreStock", x => x.stock_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbStoreStock");

            migrationBuilder.DropColumn(
                name: "CY_amount",
                table: "dbItemMaster");

            migrationBuilder.DropColumn(
                name: "CY_qty",
                table: "dbItemMaster");

            migrationBuilder.DropColumn(
                name: "LY_amount",
                table: "dbItemMaster");

            migrationBuilder.DropColumn(
                name: "LY_qty",
                table: "dbItemMaster");

            migrationBuilder.DropColumn(
                name: "month_age",
                table: "dbItemMaster");

            migrationBuilder.DropColumn(
                name: "month_amount",
                table: "dbItemMaster");

            migrationBuilder.DropColumn(
                name: "month_qty",
                table: "dbItemMaster");

            migrationBuilder.DropColumn(
                name: "year_age",
                table: "dbItemMaster");
        }
    }
}
