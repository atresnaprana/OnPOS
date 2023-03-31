using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addsizecomponentstore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "STORAGE_VAL",
                table: "xstore_organization",
                type: "decimal(11,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SELLING_VAL",
                table: "xstore_organization",
                type: "decimal(11,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "GROSS_VAL",
                table: "xstore_organization",
                type: "decimal(11,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "STORAGE_VAL",
                table: "xstore_organization",
                type: "decimal(5,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SELLING_VAL",
                table: "xstore_organization",
                type: "decimal(5,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "GROSS_VAL",
                table: "xstore_organization",
                type: "decimal(5,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,2)",
                oldNullable: true);
        }
    }
}
