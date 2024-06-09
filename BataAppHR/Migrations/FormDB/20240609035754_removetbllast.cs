using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataAppHR.Migrations.FormDB
{
    public partial class removetbllast : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbArticle");

            migrationBuilder.DropTable(
                name: "dbNilaiSS");

            migrationBuilder.DropTable(
                name: "dbNilaiSSFixed");

            migrationBuilder.DropTable(
                name: "dbOrderConfig");

            migrationBuilder.DropTable(
                name: "dbPaymentList");

            migrationBuilder.DropTable(
                name: "dbProgram");

            migrationBuilder.DropTable(
                name: "dbProgram_seq");

            migrationBuilder.DropTable(
                name: "dbRekapTraining");

            migrationBuilder.DropTable(
                name: "dbRekapTraining_seq");

            migrationBuilder.DropTable(
                name: "dbRekapTrainingFixed");

            migrationBuilder.DropTable(
                name: "dbSalesCreditLog");

            migrationBuilder.DropTable(
                name: "dbSalesOrder");

            migrationBuilder.DropTable(
                name: "dbSalesOrderDtl");

            migrationBuilder.DropTable(
                name: "dbSalesOrderDtlTemp");

            migrationBuilder.DropTable(
                name: "dbsalesorderpicking");

            migrationBuilder.DropTable(
                name: "dbsalesorderpickingref");

            migrationBuilder.DropTable(
                name: "dbSalesOrderTemp");

            migrationBuilder.DropTable(
                name: "dbSalesWholeSale");

            migrationBuilder.DropTable(
                name: "SalesOrderDtlCreditTbl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dbArticle",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Article = table.Column<string>(type: "varchar(7)", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    ENTRY_USER = table.Column<string>(type: "varchar(80)", nullable: true),
                    FILE_IMG = table.Column<byte[]>(type: "MediumBlob", nullable: true),
                    FILE_IMG_NAME = table.Column<string>(type: "varchar(255)", nullable: true),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATE_USER = table.Column<string>(type: "varchar(80)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbNilaiSS",
                columns: table => new
                {
                    ScoreId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EMP_TYPE = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Entry_Date = table.Column<DateTime>(type: "date", nullable: false),
                    Entry_User = table.Column<string>(type: "varchar(100)", nullable: true),
                    FILE_SERTIFIKAT = table.Column<string>(type: "varchar(255)", nullable: true),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true),
                    NILAI = table.Column<int>(type: "int", nullable: false),
                    NoSertifikat = table.Column<string>(type: "varchar(255)", nullable: true),
                    SERTIFIKAT = table.Column<string>(type: "varchar(1)", nullable: false),
                    SS_CODE = table.Column<string>(type: "varchar(255)", nullable: true),
                    TRN_ID = table.Column<string>(type: "varchar(255)", nullable: true),
                    Update_Date = table.Column<DateTime>(type: "date", nullable: false),
                    Update_User = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreTbl", x => x.ScoreId);
                });

            migrationBuilder.CreateTable(
                name: "dbNilaiSSFixed",
                columns: table => new
                {
                    ScoreId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EMP_TYPE = table.Column<string>(type: "varchar(255)", nullable: true),
                    Entry_Date = table.Column<DateTime>(type: "date", nullable: false),
                    Entry_User = table.Column<string>(type: "varchar(100)", nullable: true),
                    FILE_SERTIFIKAT = table.Column<string>(type: "varchar(255)", nullable: true),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true),
                    ISPRESENT = table.Column<string>(type: "varchar(1)", nullable: true),
                    NILAI = table.Column<int>(type: "int", nullable: false),
                    NoSertifikat = table.Column<string>(type: "varchar(255)", nullable: true),
                    SERTIFIKAT = table.Column<string>(type: "varchar(1)", nullable: false),
                    SS_CODE = table.Column<string>(type: "varchar(255)", nullable: false),
                    TRN_ID = table.Column<string>(type: "varchar(255)", nullable: false),
                    Update_Date = table.Column<DateTime>(type: "date", nullable: false),
                    Update_User = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreTblFixed", x => x.ScoreId);
                });

            migrationBuilder.CreateTable(
                name: "dbOrderConfig",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    ENTRY_USER = table.Column<string>(type: "varchar(50)", nullable: true),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATE_USER = table.Column<string>(type: "varchar(10)", nullable: false),
                    pairsarticle = table.Column<int>(type: "int", nullable: false),
                    pairsorder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderConfig", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbPaymentList",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BANK = table.Column<string>(type: "varchar(255)", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    ENTRY_USER = table.Column<string>(type: "varchar(80)", nullable: true),
                    FILE_PAYMENT = table.Column<byte[]>(type: "MediumBlob", nullable: true),
                    FILE_PAYMENT_NAME = table.Column<string>(type: "varchar(255)", nullable: true),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true),
                    PAYMENT_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    REF_ID = table.Column<string>(type: "varchar(255)", nullable: true),
                    TOTAL_PAY = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATE_USER = table.Column<string>(type: "varchar(80)", nullable: true),
                    id_customer = table.Column<int>(type: "int(10)", nullable: false),
                    id_order = table.Column<int>(type: "int(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentList", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbProgram",
                columns: table => new
                {
                    ProgramId = table.Column<string>(type: "varchar(50)", nullable: false),
                    Entry_Date = table.Column<DateTime>(type: "date", nullable: false),
                    Entry_User = table.Column<string>(type: "varchar(100)", nullable: true),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true),
                    ProgramName = table.Column<string>(type: "varchar(255)", nullable: true),
                    Update_Date = table.Column<DateTime>(type: "date", nullable: false),
                    Update_User = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramTbl", x => x.ProgramId);
                });

            migrationBuilder.CreateTable(
                name: "dbProgram_seq",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Program_Seq", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbRekapTraining",
                columns: table => new
                {
                    TRN_ID = table.Column<string>(type: "varchar(50)", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: true),
                    Days = table.Column<int>(type: "int", nullable: false),
                    EDP = table.Column<string>(type: "varchar(255)", nullable: true),
                    Entry_Date = table.Column<DateTime>(type: "date", nullable: false),
                    Entry_User = table.Column<string>(type: "varchar(100)", nullable: true),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true),
                    Hours = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    NoParticipant = table.Column<int>(type: "int", nullable: false),
                    Participant = table.Column<string>(type: "varchar(255)", nullable: true),
                    Periode = table.Column<string>(type: "varchar(255)", nullable: true),
                    Program = table.Column<string>(type: "varchar(255)", nullable: true),
                    ProgramTxt = table.Column<string>(type: "varchar(255)", nullable: true),
                    TotalHours = table.Column<int>(type: "int", nullable: false),
                    Trainer = table.Column<string>(type: "varchar(255)", nullable: true),
                    Type = table.Column<string>(type: "varchar(255)", nullable: true),
                    Update_Date = table.Column<DateTime>(type: "date", nullable: false),
                    Update_User = table.Column<string>(type: "varchar(100)", nullable: true),
                    Week = table.Column<string>(type: "varchar(255)", nullable: true),
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idTrainer = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RekapTrainingTbl", x => x.TRN_ID);
                });

            migrationBuilder.CreateTable(
                name: "dbRekapTraining_seq",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RekapTraining_Seq", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbRekapTrainingFixed",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Batch = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "date", nullable: true),
                    Days = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true),
                    EDP = table.Column<string>(type: "varchar(255)", nullable: true),
                    Entry_Date = table.Column<DateTime>(type: "date", nullable: true),
                    Entry_User = table.Column<string>(type: "varchar(100)", nullable: true),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true),
                    Hours = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true),
                    NoParticipant = table.Column<int>(type: "int", nullable: true),
                    Participant = table.Column<string>(type: "varchar(255)", nullable: true),
                    Periode = table.Column<string>(type: "varchar(255)", nullable: true),
                    Program = table.Column<string>(type: "varchar(255)", nullable: true),
                    ProgramTxt = table.Column<string>(type: "varchar(255)", nullable: true),
                    TRN_ID = table.Column<string>(type: "varchar(50)", nullable: true),
                    TotalHours = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true),
                    Trainer = table.Column<string>(type: "varchar(255)", nullable: true),
                    TrainingDays = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "varchar(255)", nullable: true),
                    Update_Date = table.Column<DateTime>(type: "date", nullable: true),
                    Update_User = table.Column<string>(type: "varchar(100)", nullable: true),
                    Week = table.Column<string>(type: "varchar(255)", nullable: true),
                    idTrainer = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RekapTrainingTblFixed", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbSalesCreditLog",
                columns: table => new
                {
                    edp = table.Column<string>(type: "varchar(15)", nullable: false),
                    invno = table.Column<string>(type: "varchar(50)", nullable: false),
                    isused = table.Column<string>(type: "varchar(1)", nullable: true),
                    valuecredit = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creditlogs", x => x.edp);
                });

            migrationBuilder.CreateTable(
                name: "dbSalesOrder",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    APPROVAL_1 = table.Column<string>(type: "varchar(10)", nullable: true),
                    APPROVAL_2 = table.Column<string>(type: "varchar(10)", nullable: true),
                    APPROVAL_AR = table.Column<string>(type: "varchar(1)", nullable: true),
                    EMAIL = table.Column<string>(type: "varchar(255)", nullable: true),
                    EMP_CODE = table.Column<string>(type: "varchar(25)", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    ENTRY_USER = table.Column<string>(type: "varchar(80)", nullable: true),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true),
                    FLAG_SENT = table.Column<string>(type: "varchar(10)", nullable: true),
                    INV_DISC = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    INV_DISC_AMT = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    IS_DISC_PERC = table.Column<string>(type: "varchar(10)", nullable: true),
                    ORDER_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    SHIPPING_ADDRESS = table.Column<string>(type: "varchar(255)", nullable: true),
                    STATUS = table.Column<string>(type: "varchar(10)", nullable: true),
                    TOTAL_ORDER = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    TOTAL_QTY = table.Column<int>(type: "int(10)", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATE_USER = table.Column<string>(type: "varchar(80)", nullable: true),
                    id_customer = table.Column<int>(type: "int(10)", nullable: true),
                    picking_no = table.Column<string>(type: "varchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrder", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbSalesOrderDtl",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Size_1 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_10 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_11 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_12 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_13 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_2 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_3 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_4 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_5 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_6 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_7 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_8 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_9 = table.Column<int>(type: "int(10)", nullable: false),
                    article = table.Column<string>(type: "varchar(10)", nullable: true),
                    disc = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    disc_amt = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    final_price = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    id_order = table.Column<int>(type: "int(10)", nullable: false),
                    is_disc_perc = table.Column<string>(type: "varchar(10)", nullable: true),
                    price = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    qty = table.Column<int>(type: "int(10)", nullable: false),
                    size = table.Column<string>(type: "varchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderDtl", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbSalesOrderDtlTemp",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Size_1 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_10 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_11 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_12 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_13 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_2 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_3 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_4 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_5 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_6 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_7 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_8 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_9 = table.Column<int>(type: "int(10)", nullable: false),
                    article = table.Column<string>(type: "varchar(10)", nullable: true),
                    disc = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    disc_amt = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    final_price = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    id_order = table.Column<int>(type: "int(10)", nullable: false),
                    is_disc_perc = table.Column<string>(type: "varchar(10)", nullable: true),
                    price = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    qty = table.Column<int>(type: "int(10)", nullable: false),
                    size = table.Column<string>(type: "varchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderDtlTemp", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbsalesorderpicking",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salesorderpicking", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbsalesorderpickingref",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salesorderpickingref", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbSalesOrderTemp",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    APPROVAL_1 = table.Column<string>(type: "varchar(10)", nullable: true),
                    APPROVAL_2 = table.Column<string>(type: "varchar(10)", nullable: true),
                    APPROVAL_AR = table.Column<string>(type: "varchar(1)", nullable: true),
                    EMAIL = table.Column<string>(type: "varchar(255)", nullable: true),
                    EMP_CODE = table.Column<string>(type: "varchar(25)", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    ENTRY_USER = table.Column<string>(type: "varchar(80)", nullable: true),
                    FLAG_AKTIF = table.Column<string>(type: "varchar(1)", nullable: true),
                    FLAG_SENT = table.Column<string>(type: "varchar(10)", nullable: true),
                    INV_DISC = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    INV_DISC_AMT = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    IS_DISC_PERC = table.Column<string>(type: "varchar(10)", nullable: true),
                    ORDER_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    SHIPPING_ADDRESS = table.Column<string>(type: "varchar(255)", nullable: true),
                    STATUS = table.Column<string>(type: "varchar(10)", nullable: true),
                    TOTAL_ORDER = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    TOTAL_QTY = table.Column<int>(type: "int(10)", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATE_USER = table.Column<string>(type: "varchar(80)", nullable: true),
                    id_customer = table.Column<int>(type: "int(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderTemp", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dbSalesWholeSale",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EMP_CODE = table.Column<string>(type: "varchar(10)", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    ENTRY_USER = table.Column<string>(type: "varchar(80)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATE_USER = table.Column<string>(type: "varchar(80)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesWhole", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SalesOrderDtlCreditTbl",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Size_1 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_10 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_11 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_12 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_13 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_2 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_3 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_4 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_5 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_6 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_7 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_8 = table.Column<int>(type: "int(10)", nullable: false),
                    Size_9 = table.Column<int>(type: "int(10)", nullable: false),
                    article = table.Column<string>(type: "varchar(10)", nullable: true),
                    disc = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    disc_amt = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    final_price = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    id_order = table.Column<int>(type: "int(10)", nullable: false),
                    is_disc_perc = table.Column<string>(type: "varchar(10)", nullable: true),
                    picking_no = table.Column<string>(type: "varchar(20)", nullable: true),
                    price = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    qty = table.Column<int>(type: "int(10)", nullable: false),
                    size = table.Column<string>(type: "varchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderDtlCredit", x => x.id);
                });
        }
    }
}
