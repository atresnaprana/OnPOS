using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class addbucketidstock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "bucket_id",
                table: "dbStoreStock",
                type: "varchar(100)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bucket_id",
                table: "dbStoreStock");
        }
    }
}
