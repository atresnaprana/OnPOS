using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addmoreflagaktifsalespayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FLAG_AKTIF",
                table: "dbSalesOrder",
                type: "varchar(1)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FLAG_AKTIF",
                table: "dbPaymentList",
                type: "varchar(1)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FLAG_AKTIF",
                table: "dbSalesOrder");

            migrationBuilder.DropColumn(
                name: "FLAG_AKTIF",
                table: "dbPaymentList");
        }
    }
}
