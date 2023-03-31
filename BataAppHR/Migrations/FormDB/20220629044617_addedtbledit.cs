using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addedtbledit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesOrderDtlCreditTbl",
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
                    table.PrimaryKey("PK_SalesOrderDtlCredit", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesOrderDtlCreditTbl");
        }
    }
}
