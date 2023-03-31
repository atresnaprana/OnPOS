using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class adddiscamt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "disc_amt",
                table: "dbSalesOrderDtl",
                type: "decimal(15,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "is_disc_perc",
                table: "dbSalesOrderDtl",
                type: "varchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "INV_DISC_AMT",
                table: "dbSalesOrder",
                type: "decimal(15,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IS_DISC_PERC",
                table: "dbSalesOrder",
                type: "varchar(10)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "disc_amt",
                table: "dbSalesOrderDtl");

            migrationBuilder.DropColumn(
                name: "is_disc_perc",
                table: "dbSalesOrderDtl");

            migrationBuilder.DropColumn(
                name: "INV_DISC_AMT",
                table: "dbSalesOrder");

            migrationBuilder.DropColumn(
                name: "IS_DISC_PERC",
                table: "dbSalesOrder");
        }
    }
}
