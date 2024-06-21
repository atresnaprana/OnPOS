using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class adddisctbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dbDiscount",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    article = table.Column<string>(type: "varchar(50)", nullable: false),
                    type = table.Column<string>(type: "varchar(255)", nullable: true),
                    percentage = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    entry_user = table.Column<string>(type: "varchar(255)", nullable: true),
                    entry_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    update_user = table.Column<string>(type: "varchar(255)", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbDiscount");
        }
    }
}
