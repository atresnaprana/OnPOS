using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class removingtblpart2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbEmployee");

            migrationBuilder.DropTable(
                name: "dbRD");

            migrationBuilder.DropTable(
                name: "dbRDSeq");

            migrationBuilder.DropTable(
                name: "dbTrainer");

            migrationBuilder.DropTable(
                name: "dbTrainer_seq");

            migrationBuilder.DropTable(
                name: "dbTrainerList");

            migrationBuilder.DropTable(
                name: "xstore_organization");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dbEmployee",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DAYS_LENGTH = table.Column<int>(type: "int", nullable: false),
                    EMERGENCY_ADDRESS = table.Column<string>(type: "varchar(300)", nullable: true),
                    EMERGENCY_NAME = table.Column<string>(type: "varchar(255)", nullable: true),
                    EMERGENCY_PHONE = table.Column<string>(type: "varchar(255)", nullable: true),
                    EMP_CODE = table.Column<string>(type: "varchar(25)", nullable: true),
                    EMP_EMAIL = table.Column<string>(type: "varchar(255)", nullable: true),
                    EMP_HP = table.Column<string>(type: "varchar(255)", nullable: true),
                    EMP_PHOTO = table.Column<string>(type: "varchar(300)", nullable: true),
                    EMP_SEPATU_SIZEUK = table.Column<string>(type: "varchar(255)", nullable: true),
                    EMP_SERAGAM_SIZE = table.Column<string>(type: "varchar(255)", nullable: true),
                    EMP_TYPE = table.Column<string>(type: "varchar(255)", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    ENTRY_USER = table.Column<string>(type: "varchar(255)", nullable: true),
                    FILE_MEDIC = table.Column<string>(type: "varchar(300)", nullable: true),
                    FILE_SERTIFIKAT1 = table.Column<string>(type: "varchar(300)", nullable: true),
                    FILE_SERTIFIKAT2 = table.Column<string>(type: "varchar(300)", nullable: true),
                    FLAG_AKTIF = table.Column<int>(type: "int", nullable: false),
                    IS_SALES_WH = table.Column<string>(type: "varchar(1)", nullable: true),
                    JOIN_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    LAMA_KERJA = table.Column<string>(type: "varchar(255)", nullable: true),
                    MONTH_LENGTH = table.Column<int>(type: "int", nullable: false),
                    NM_EMP = table.Column<string>(type: "varchar(255)", nullable: true),
                    No_KTP = table.Column<string>(type: "varchar(255)", nullable: true),
                    POSITION = table.Column<string>(type: "varchar(255)", nullable: true),
                    RESIGN_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    RESIGN_TXT = table.Column<string>(type: "varchar(255)", nullable: true),
                    RESIGN_TYPE = table.Column<string>(type: "varchar(255)", nullable: true),
                    RESIGN_TYPE2 = table.Column<string>(type: "varchar(255)", nullable: true),
                    SEX = table.Column<string>(type: "varchar(255)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    UPDATE_USER = table.Column<string>(type: "varchar(255)", nullable: true),
                    VAKSIN1 = table.Column<string>(type: "varchar(255)", nullable: true),
                    VAKSIN2 = table.Column<string>(type: "varchar(255)", nullable: true),
                    YEAR_LENGTH = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbEmp_Num", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbRD",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DAYS_LENGTH = table.Column<int>(type: "int", nullable: false),
                    EDP_CODE = table.Column<string>(type: "varchar(5)", nullable: true),
                    EMERGENCY_ADDRESS = table.Column<string>(type: "varchar(300)", nullable: true),
                    EMERGENCY_NAME = table.Column<string>(type: "varchar(255)", nullable: true),
                    EMERGENCY_PHONE = table.Column<string>(type: "varchar(255)", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    ENTRY_USER = table.Column<string>(type: "varchar(255)", nullable: true),
                    FILE_MEDIC = table.Column<string>(type: "varchar(300)", nullable: true),
                    FILE_SERTIFIKAT1 = table.Column<string>(type: "varchar(300)", nullable: true),
                    FILE_SERTIFIKAT2 = table.Column<string>(type: "varchar(300)", nullable: true),
                    FLAG_AKTIF = table.Column<int>(type: "int", nullable: false),
                    JOIN_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    LAMA_KERJA = table.Column<string>(type: "varchar(255)", nullable: true),
                    MONTH_LENGTH = table.Column<int>(type: "int", nullable: false),
                    NM_RD = table.Column<string>(type: "varchar(255)", nullable: true),
                    No_KTP = table.Column<string>(type: "varchar(255)", nullable: true),
                    RD_CODE = table.Column<string>(type: "varchar(10)", nullable: true),
                    RD_EMAIL = table.Column<string>(type: "varchar(255)", nullable: true),
                    RD_HP = table.Column<string>(type: "varchar(255)", nullable: true),
                    RD_PHOTO = table.Column<string>(type: "varchar(300)", nullable: true),
                    RD_SEPATU_SIZEUK = table.Column<string>(type: "varchar(255)", nullable: true),
                    RD_SERAGAM_SIZE = table.Column<string>(type: "varchar(255)", nullable: true),
                    RESIGN_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    RESIGN_TXT = table.Column<string>(type: "varchar(255)", nullable: true),
                    RESIGN_TYPE = table.Column<string>(type: "varchar(255)", nullable: true),
                    RESIGN_TYPE2 = table.Column<string>(type: "varchar(255)", nullable: true),
                    SEX = table.Column<string>(type: "varchar(255)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    UPDATE_USER = table.Column<string>(type: "varchar(255)", nullable: true),
                    VAKSIN1 = table.Column<string>(type: "varchar(255)", nullable: true),
                    VAKSIN2 = table.Column<string>(type: "varchar(255)", nullable: true),
                    YEAR_LENGTH = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RD_num", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbRDSeq",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RD_Seq", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbTrainer",
                columns: table => new
                {
                    idTrainer = table.Column<string>(type: "varchar(50)", nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", nullable: true),
                    Entry_Date = table.Column<DateTime>(type: "date", nullable: false),
                    Entry_User = table.Column<string>(type: "varchar(100)", nullable: true),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true),
                    HP = table.Column<string>(type: "varchar(255)", nullable: true),
                    NmShort = table.Column<string>(type: "varchar(255)", nullable: true),
                    NmTrainer = table.Column<string>(type: "varchar(255)", nullable: true),
                    Update_Date = table.Column<DateTime>(type: "date", nullable: false),
                    Update_User = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerTbl", x => x.idTrainer);
                });

            migrationBuilder.CreateTable(
                name: "dbTrainer_seq",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainer_Seq", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbTrainerList",
                columns: table => new
                {
                    idTrainerList = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idFormRekap = table.Column<int>(type: "int", nullable: false),
                    idTrainer = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_idTrainerList", x => x.idTrainerList);
                });

            migrationBuilder.CreateTable(
                name: "xstore_organization",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ADDRESS = table.Column<string>(type: "varchar(255)", nullable: true),
                    CLOSE_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    DEPT_COMM = table.Column<double>(type: "double", nullable: false),
                    DEPT_COMM_PERC = table.Column<decimal>(type: "decimal(11,2)", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    ENTRY_USER = table.Column<string>(type: "varchar(255)", nullable: true),
                    FILE_LEASE = table.Column<string>(type: "varchar(255)", nullable: true),
                    FILE_OTHERS = table.Column<string>(type: "varchar(255)", nullable: true),
                    FLAG_APPROVAL = table.Column<int>(type: "int", nullable: true),
                    GROSS_VAL = table.Column<decimal>(type: "decimal(11,2)", nullable: true),
                    IS_DS = table.Column<string>(type: "varchar(1)", nullable: true),
                    IS_PERC = table.Column<string>(type: "varchar(1)", nullable: true),
                    IS_PERC_DEPT = table.Column<string>(type: "varchar(1)", nullable: true),
                    LAST_RENOVATION_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    LEASE_EXPIRED = table.Column<DateTime>(type: "date", nullable: true),
                    LEASE_START = table.Column<DateTime>(type: "date", nullable: true),
                    OPENING_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    RD_CODE = table.Column<string>(type: "varchar(10)", nullable: true),
                    RD_COMM = table.Column<double>(type: "double", nullable: false),
                    RD_COMM_PERC = table.Column<decimal>(type: "decimal(11,2)", nullable: true),
                    SELLING_AREA = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    SELLING_VAL = table.Column<decimal>(type: "decimal(11,2)", nullable: true),
                    STOCK_AREA = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    STORAGE_VAL = table.Column<decimal>(type: "decimal(11,2)", nullable: true),
                    STORE_CONCEPT = table.Column<string>(type: "varchar(150)", nullable: true),
                    STORE_IMAGE = table.Column<string>(type: "varchar(255)", nullable: true),
                    STORE_IMG_BLOB = table.Column<byte[]>(type: "MediumBlob", nullable: true),
                    TOTAL_AREA = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    UPDATE_USER = table.Column<string>(type: "varchar(255)", nullable: true),
                    area = table.Column<string>(type: "varchar(50)", nullable: true),
                    district = table.Column<string>(type: "varchar(5)", nullable: true),
                    edp = table.Column<string>(type: "varchar(5)", nullable: true),
                    genesis_Flag = table.Column<string>(type: "varchar(50)", nullable: true),
                    inactive_flag = table.Column<string>(type: "varchar(1)", nullable: true),
                    store_location = table.Column<string>(type: "varchar(75)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_xstore_organizations_num", x => x.id);
                });
        }
    }
}
