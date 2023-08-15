using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataAppHR.Models;
using OnPOS.Models;

using Microsoft.EntityFrameworkCore;

namespace BataAppHR.Data
{
    public class FormDBContext : DbContext
    {
        public DbSet<VaksinModel> SSTable { get; set; }
        public DbSet<XstoreModel> xstore_organization { get; set; }
        public DbSet<dbRD> RDTbl { get; set; }
        public DbSet<dbEmployee> EmpTbl { get; set; }
        public DbSet<dbTrainer> trainerDb { get; set; }
        public DbSet<dbRekapTraining> rekapDb { get; set; }
        public DbSet<dbNilaiSS> NilaiSSTbl { get; set; }
        public DbSet<dbNilaiSSFixed> NilaissTblFixed { get; set; }
        public DbSet<dbrekapTrainingfixed> rekapdbFixed { get; set; }
        public DbSet<dbProgram> programDb { get; set; }

        public DbSet<SystemTabModel> TabTbl { get; set; }
        public DbSet<SystemMenuModel> MenuTbl { get; set; }
        public DbSet<dbSliderImg> SlideTbl { get; set; }
        public DbSet<dbTrainerList> trainerlistTbl { get; set; }


        public DbSet<dbCustomer> CustomerTbl { get; set; }
        public DbSet<dbSalesWholeSale> SalesWholeSaleTbl { get; set; }
        public DbSet<dbStoreList> StoreListTbl { get; set; }

        public DbSet<dbSalesOrder> SalesOrderTbl { get; set; }
        public DbSet<dbSalesOrderDtl> SalesOrderDtlTbl { get; set; }
        public DbSet<dbPaymentList> PaymentTbl { get; set; }
        public DbSet<dbArticle> ArticleTbl { get; set; }
        public DbSet<dbOrderConfig> OrderConfigTbl { get; set; }

        public DbSet<dbSalesOrderTemp> SalesOrderTempTbl { get; set; }
        public DbSet<dbSalesOrderDtlTemp> SalesOrderDtlTempTbl { get; set; }
        public DbSet<dbSalesOrderDtlCredit> SalesOrderDtlCreditTbl { get; set; }
        public DbSet<dbsalescreditlog> creditlogtbl { get; set; }


        public FormDBContext(DbContextOptions<FormDBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use Fluent API to configure  

            // Map entities to tables  
            modelBuilder.Entity<VaksinModel>().ToTable("SSTable");
            modelBuilder.Entity<VaksinSeqModel>().ToTable("sstable_seq");
            modelBuilder.Entity<dbTrainerSeq>().ToTable("dbTrainer_seq");
            modelBuilder.Entity<dbProgramSeq>().ToTable("dbProgram_seq");
            modelBuilder.Entity<dbCompanySeq>().ToTable("dbCompany_seq");

            modelBuilder.Entity<dbRekapTrainingSeq>().ToTable("dbRekapTraining_seq");
            modelBuilder.Entity<dbCustomerSeq>().ToTable("dbCustomer_seq");

            modelBuilder.Entity<XstoreModel>().ToTable("xstore_organization");
            modelBuilder.Entity<dbRD>().ToTable("dbRD");
            modelBuilder.Entity<dbEmployee>().ToTable("dbEmployee");

            modelBuilder.Entity<SystemTabModel>().ToTable("SystemTabTbl");
            modelBuilder.Entity<SystemMenuModel>().ToTable("SystemMenuTbl");
            modelBuilder.Entity<dbTrainer>().ToTable("dbTrainer");
            modelBuilder.Entity<dbProgram>().ToTable("dbProgram");
            modelBuilder.Entity<dbRekapTraining>().ToTable("dbRekapTraining");
            modelBuilder.Entity<dbrekapTrainingfixed>().ToTable("dbRekapTrainingFixed");
            modelBuilder.Entity<dbSliderImg>().ToTable("dbSliderImg");

            modelBuilder.Entity<dbNilaiSS>().ToTable("dbNilaiSS");
            modelBuilder.Entity<dbNilaiSSFixed>().ToTable("dbNilaiSSFixed");
            modelBuilder.Entity<dbTrainerList>().ToTable("dbTrainerList");
            modelBuilder.Entity<dbCustomer>().ToTable("dbCustomer");
            modelBuilder.Entity<dbSalesWholeSale>().ToTable("dbSalesWholeSale");
            modelBuilder.Entity<dbSalesOrder>().ToTable("dbSalesOrder");
            modelBuilder.Entity<dbSalesOrderDtl>().ToTable("dbSalesOrderDtl");
            modelBuilder.Entity<dbPaymentList>().ToTable("dbPaymentList");
            modelBuilder.Entity<dbArticle>().ToTable("dbArticle");
            modelBuilder.Entity<dbOrderConfig>().ToTable("dbOrderConfig");
            modelBuilder.Entity<dbSalesOrderTemp>().ToTable("dbSalesOrderTemp");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().ToTable("dbSalesOrderDtlTemp");
            modelBuilder.Entity<dbsalescreditlog>().ToTable("dbSalesCreditLog").HasKey(t => new { t.edp, t.invno });
            modelBuilder.Entity<dbStoreList>().ToTable("dbStoreList");

            // Configure Primary Keys  
            modelBuilder.Entity<VaksinModel>().HasKey(ug => ug.ID).HasName("PK_SSTable");
            modelBuilder.Entity<dbRD>().HasKey(ug => ug.id).HasName("PK_RD_num");
            modelBuilder.Entity<dbTrainer>().HasKey(ug => ug.idTrainer).HasName("PK_TrainerTbl");
            modelBuilder.Entity<dbProgram>().HasKey(ug => ug.ProgramId).HasName("PK_ProgramTbl");
            modelBuilder.Entity<dbRekapTraining>().HasKey(ug => ug.TRN_ID).HasName("PK_RekapTrainingTbl");
            modelBuilder.Entity<dbrekapTrainingfixed>().HasKey(ug => ug.id).HasName("PK_RekapTrainingTblFixed");
            modelBuilder.Entity<dbTrainerList>().HasKey(ug => ug.idTrainerList).HasName("PK_idTrainerList");


            modelBuilder.Entity<dbNilaiSS>().HasKey(ug => ug.ScoreId).HasName("PK_ScoreTbl");
            modelBuilder.Entity<dbNilaiSSFixed>().HasKey(ug => ug.ScoreId).HasName("PK_ScoreTblFixed");

            modelBuilder.Entity<VaksinSeqModel>().HasKey(ug => ug.id).HasName("PK_SSTable_Seq");
            modelBuilder.Entity<dbRDSeq>().HasKey(ug => ug.id).HasName("PK_RD_Seq");
            modelBuilder.Entity<dbEmployee>().HasKey(ug => ug.id).HasName("PK_dbEmp_Num");

            modelBuilder.Entity<SystemTabModel>().HasKey(ug => ug.ID).HasName("PK_TAB_Seq");
            modelBuilder.Entity<SystemMenuModel>().HasKey(ug => ug.ID).HasName("PK_MENU_Seq");
            modelBuilder.Entity<dbTrainerSeq>().HasKey(ug => ug.id).HasName("PK_Trainer_Seq");
            modelBuilder.Entity<dbProgramSeq>().HasKey(ug => ug.id).HasName("PK_Program_Seq");
            modelBuilder.Entity<dbRekapTrainingSeq>().HasKey(ug => ug.id).HasName("PK_RekapTraining_Seq");
            modelBuilder.Entity<dbCompanySeq>().HasKey(ug => ug.id).HasName("PK_Company_Seq");

            modelBuilder.Entity<XstoreModel>().HasKey(ug => ug.edp).HasName("PK_xstore_organizations");

            modelBuilder.Entity<XstoreModel>().HasKey(ug => ug.id).HasName("PK_xstore_organizations_num");
            modelBuilder.Entity<dbSliderImg>().HasKey(ug => ug.ID).HasName("PK_SliderImg");
            modelBuilder.Entity<dbCustomer>().HasKey(ug => ug.id).HasName("PK_Customer");
            modelBuilder.Entity<dbSalesWholeSale>().HasKey(ug => ug.id).HasName("PK_SalesWhole");
            modelBuilder.Entity<dbSalesOrder>().HasKey(ug => ug.id).HasName("PK_SalesOrder");
            modelBuilder.Entity<dbSalesOrderDtl>().HasKey(ug => ug.id).HasName("PK_SalesOrderDtl");
            modelBuilder.Entity<dbPaymentList>().HasKey(ug => ug.id).HasName("PK_PaymentList");
            modelBuilder.Entity<dbCustomerSeq>().HasKey(ug => ug.id).HasName("PK_Cust_Seq");
            modelBuilder.Entity<dbArticle>().HasKey(ug => ug.id).HasName("PK_Article");
            modelBuilder.Entity<dbOrderConfig>().HasKey(ug => ug.id).HasName("PK_OrderConfig");
            modelBuilder.Entity<dbSalesOrderTemp>().HasKey(ug => ug.id).HasName("PK_SalesOrderTemp");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().HasKey(ug => ug.id).HasName("PK_SalesOrderDtlTemp");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().HasKey(ug => ug.id).HasName("PK_SalesOrderDtlCredit");
            modelBuilder.Entity<dbsalesorderpicking>().HasKey(ug => ug.id).HasName("PK_salesorderpicking");
            modelBuilder.Entity<dbsalesorderpickingref>().HasKey(ug => ug.id).HasName("PK_salesorderpickingref");
            modelBuilder.Entity<dbStoreList>().HasKey(ug => ug.id).HasName("PK_StoreList");

            // Configure indexes  
            //modelBuilder.Entity<UserGroup>().HasIndex(p => p.Name).IsUnique().HasDatabaseName("Idx_Name");  
            //modelBuilder.Entity<User>().HasIndex(u => u.FirstName).HasDatabaseName("Idx_FirstName");  
            //modelBuilder.Entity<User>().HasIndex(u => u.LastName).HasDatabaseName("Idx_LastName");  

            // Configure columns  
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.ID).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.SS_CODE).HasColumnType("varchar(255)");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.NAMA_SS).HasColumnType("varchar(255)");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.FLAG_AKTIF).HasColumnType("int");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.SEX).HasColumnType("varchar(255)");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.KTP).HasColumnType("varchar(255)");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.HP_SS).HasColumnType("varchar(255)");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.EMAIL_SS).HasColumnType("varchar(255)");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.SIZE_SERAGAM).HasColumnType("varchar(255)");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.SIZE_SEPATU_UK).HasColumnType("varchar(255)");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.RESIGN_DATE).HasColumnType("date");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.JOIN_DATE).HasColumnType("date");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.RESIGN_TXT).HasColumnType("varchar(255)");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.LAMA_KERJA).HasColumnType("varchar(255)");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.VAKSIN1).HasColumnType("varchar(255)");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.VAKSIN2).HasColumnType("varchar(255)");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.FOTOSERTIFIKAT1).HasColumnType("varchar(300)");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.FOTOSERTIFIKAT2).HasColumnType("varchar(300)");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.ENTRY_DATE).HasColumnType("date");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.UPDATE_DATE).HasColumnType("date");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.UPDATE_USER).HasColumnType("varchar(255)");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.ENTRY_USER).HasColumnType("varchar(255)");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.EDP_CODE).HasColumnType("varchar(5)");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.STAFF_PHOTO).HasColumnType("varchar(300)").IsRequired(false);
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.EMERGENCY_NAME).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.EMERGENCY_PHONE).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.EMERGENCY_ADDRESS).HasColumnType("varchar(300)").IsRequired(false);
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.RESIGN_TYPE).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.RESIGN_TYPE2).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.POSITION).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.YEAR_LENGTH).HasColumnType("int");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.MONTH_LENGTH).HasColumnType("int");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.DAYS_LENGTH).HasColumnType("int");
            modelBuilder.Entity<VaksinModel>().Property(ug => ug.XSTORE_LOGIN).HasColumnType("varchar(255)").IsRequired(false);

            //Model Sequence
            modelBuilder.Entity<VaksinSeqModel>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbRDSeq>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbTrainerSeq>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbProgramSeq>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbRekapTrainingSeq>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbCustomerSeq>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbsalesorderpicking>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbsalesorderpickingref>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbCompanySeq>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();


            //Model xstore
            //modelBuilder.Entity<XstoreModel>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.edp).HasColumnType("varchar(5)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.district).HasColumnType("varchar(5)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.store_location).HasColumnType("varchar(75)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.area).HasColumnType("varchar(50)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.inactive_flag).HasColumnType("varchar(1)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.genesis_Flag).HasColumnType("varchar(50)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.RD_CODE).HasColumnType("varchar(10)").IsRequired(false);

            modelBuilder.Entity<XstoreModel>().Property(ug => ug.STORE_CONCEPT).HasColumnType("varchar(150)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.SELLING_AREA).HasColumnType("decimal(5,2)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.STOCK_AREA).HasColumnType("decimal(5,2)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.TOTAL_AREA).HasColumnType("decimal(5,2)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.ADDRESS).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.OPENING_DATE).HasColumnType("date").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.LEASE_START).HasColumnType("date").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.LEASE_EXPIRED).HasColumnType("date").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.LAST_RENOVATION_DATE).HasColumnType("date").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.FILE_LEASE).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.STORE_IMAGE).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.GROSS_VAL).HasColumnType("decimal(11,2)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.SELLING_VAL).HasColumnType("decimal(11,2)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.STORAGE_VAL).HasColumnType("decimal(11,2)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.FILE_OTHERS).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.STORE_IMG_BLOB).HasColumnType("MediumBlob").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.FLAG_APPROVAL).HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.CLOSE_DATE).HasColumnType("date").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.IS_DS).HasColumnType("varchar(1)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.RD_COMM).HasColumnType("double");
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.DEPT_COMM).HasColumnType("double");
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.IS_PERC).HasColumnType("varchar(1)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.RD_COMM_PERC).HasColumnType("decimal(11,2)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.DEPT_COMM_PERC).HasColumnType("decimal(11,2)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.IS_PERC_DEPT).HasColumnType("varchar(1)").IsRequired(false);
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.ENTRY_DATE).HasColumnType("date");
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.UPDATE_DATE).HasColumnType("date");
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.UPDATE_USER).HasColumnType("varchar(255)");
            modelBuilder.Entity<XstoreModel>().Property(ug => ug.ENTRY_USER).HasColumnType("varchar(255)");

            // Configure columns RD
            modelBuilder.Entity<dbRD>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbRD>().Property(ug => ug.RD_CODE).HasColumnType("varchar(10)");
            modelBuilder.Entity<dbRD>().Property(ug => ug.EDP_CODE).HasColumnType("varchar(5)");
            modelBuilder.Entity<dbRD>().Property(ug => ug.NM_RD).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbRD>().Property(ug => ug.FLAG_AKTIF).HasColumnType("int");
            modelBuilder.Entity<dbRD>().Property(ug => ug.RESIGN_DATE).HasColumnType("date");
            modelBuilder.Entity<dbRD>().Property(ug => ug.RESIGN_TXT).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbRD>().Property(ug => ug.SEX).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbRD>().Property(ug => ug.RD_HP).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbRD>().Property(ug => ug.RD_EMAIL).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbRD>().Property(ug => ug.JOIN_DATE).HasColumnType("date");
            modelBuilder.Entity<dbRD>().Property(ug => ug.RD_SERAGAM_SIZE).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbRD>().Property(ug => ug.RD_SEPATU_SIZEUK).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbRD>().Property(ug => ug.No_KTP).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbRD>().Property(ug => ug.VAKSIN1).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbRD>().Property(ug => ug.VAKSIN2).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbRD>().Property(ug => ug.FILE_SERTIFIKAT1).HasColumnType("varchar(300)");
            modelBuilder.Entity<dbRD>().Property(ug => ug.FILE_SERTIFIKAT2).HasColumnType("varchar(300)");
            modelBuilder.Entity<dbRD>().Property(ug => ug.ENTRY_DATE).HasColumnType("date");
            modelBuilder.Entity<dbRD>().Property(ug => ug.UPDATE_DATE).HasColumnType("date");
            modelBuilder.Entity<dbRD>().Property(ug => ug.UPDATE_USER).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbRD>().Property(ug => ug.ENTRY_USER).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbRD>().Property(ug => ug.RD_PHOTO).HasColumnType("varchar(300)").IsRequired(false);
            modelBuilder.Entity<dbRD>().Property(ug => ug.EMERGENCY_NAME).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbRD>().Property(ug => ug.EMERGENCY_PHONE).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbRD>().Property(ug => ug.EMERGENCY_ADDRESS).HasColumnType("varchar(300)").IsRequired(false);
            modelBuilder.Entity<dbRD>().Property(ug => ug.RESIGN_TYPE).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbRD>().Property(ug => ug.RESIGN_TYPE2).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbRD>().Property(ug => ug.FILE_MEDIC).HasColumnType("varchar(300)").IsRequired(false);
            modelBuilder.Entity<dbRD>().Property(ug => ug.YEAR_LENGTH).HasColumnType("int");
            modelBuilder.Entity<dbRD>().Property(ug => ug.MONTH_LENGTH).HasColumnType("int");
            modelBuilder.Entity<dbRD>().Property(ug => ug.DAYS_LENGTH).HasColumnType("int");
            modelBuilder.Entity<dbRD>().Property(ug => ug.LAMA_KERJA).HasColumnType("varchar(255)");

            // Configure columns Employee
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.EMP_CODE).HasColumnType("varchar(25)");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.EMP_TYPE).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.NM_EMP).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.FLAG_AKTIF).HasColumnType("int");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.RESIGN_DATE).HasColumnType("date");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.RESIGN_TXT).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.SEX).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.EMP_HP).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.EMP_EMAIL).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.JOIN_DATE).HasColumnType("date");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.EMP_SERAGAM_SIZE).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.EMP_SEPATU_SIZEUK).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.No_KTP).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.VAKSIN1).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.VAKSIN2).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.FILE_SERTIFIKAT1).HasColumnType("varchar(300)");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.FILE_SERTIFIKAT2).HasColumnType("varchar(300)");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.ENTRY_DATE).HasColumnType("date");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.UPDATE_DATE).HasColumnType("date");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.UPDATE_USER).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.ENTRY_USER).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.EMP_PHOTO).HasColumnType("varchar(300)").IsRequired(false);
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.EMERGENCY_NAME).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.EMERGENCY_PHONE).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.EMERGENCY_ADDRESS).HasColumnType("varchar(300)").IsRequired(false);
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.RESIGN_TYPE).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.RESIGN_TYPE2).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.FILE_MEDIC).HasColumnType("varchar(300)").IsRequired(false);
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.YEAR_LENGTH).HasColumnType("int");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.MONTH_LENGTH).HasColumnType("int");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.DAYS_LENGTH).HasColumnType("int");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.LAMA_KERJA).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.POSITION).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbEmployee>().Property(ug => ug.IS_SALES_WH).HasColumnType("varchar(1)");

            //SystemTabTbl
            modelBuilder.Entity<SystemTabModel>().Property(ug => ug.ID).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<SystemTabModel>().Property(ug => ug.TAB_DESC).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<SystemTabModel>().Property(ug => ug.ROLE_ID).HasColumnType("varchar(75)").IsRequired(false);
            modelBuilder.Entity<SystemTabModel>().Property(ug => ug.TAB_TXT).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<SystemTabModel>().Property(ug => ug.FLAG_AKTIF).HasColumnType("varchar(1)").IsRequired(false);
            modelBuilder.Entity<SystemTabModel>().Property(ug => ug.ENTRY_USER).HasColumnType("varchar(50)").IsRequired(false);
            modelBuilder.Entity<SystemTabModel>().Property(ug => ug.UPDATE_USER).HasColumnType("varchar(10)").IsRequired();
            modelBuilder.Entity<SystemTabModel>().Property(ug => ug.ENTRY_DATE).HasColumnType("date");
            modelBuilder.Entity<SystemTabModel>().Property(ug => ug.UPDATE_DATE).HasColumnType("date");

            //SystemMenuTbl
            modelBuilder.Entity<SystemMenuModel>().Property(ug => ug.ID).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<SystemMenuModel>().Property(ug => ug.TAB_ID).HasColumnType("int");
            modelBuilder.Entity<SystemMenuModel>().Property(ug => ug.MENU_DESC).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<SystemMenuModel>().Property(ug => ug.ROLE_ID).HasColumnType("varchar(75)").IsRequired(false);
            modelBuilder.Entity<SystemMenuModel>().Property(ug => ug.MENU_TXT).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<SystemMenuModel>().Property(ug => ug.FLAG_AKTIF).HasColumnType("varchar(1)").IsRequired(false);
            modelBuilder.Entity<SystemMenuModel>().Property(ug => ug.ENTRY_USER).HasColumnType("varchar(50)").IsRequired(false);
            modelBuilder.Entity<SystemMenuModel>().Property(ug => ug.UPDATE_USER).HasColumnType("varchar(10)").IsRequired();
            modelBuilder.Entity<SystemMenuModel>().Property(ug => ug.ENTRY_DATE).HasColumnType("date");
            modelBuilder.Entity<SystemMenuModel>().Property(ug => ug.UPDATE_DATE).HasColumnType("date");

            //ModelMasterTrainer
            modelBuilder.Entity<dbTrainer>().Property(ug => ug.idTrainer).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbTrainer>().Property(ug => ug.NmTrainer).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbTrainer>().Property(ug => ug.NmShort).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbTrainer>().Property(ug => ug.HP).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbTrainer>().Property(ug => ug.Email).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbTrainer>().Property(ug => ug.Entry_Date).HasColumnType("date");
            modelBuilder.Entity<dbTrainer>().Property(ug => ug.Entry_User).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbTrainer>().Property(ug => ug.Update_Date).HasColumnType("date");
            modelBuilder.Entity<dbTrainer>().Property(ug => ug.Update_User).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbTrainer>().Property(ug => ug.FLAG_AKTIF).HasColumnType("varchar(1)");

            //ModelMasterProgram
            modelBuilder.Entity<dbProgram>().Property(ug => ug.ProgramId).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbProgram>().Property(ug => ug.ProgramName).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbProgram>().Property(ug => ug.Entry_Date).HasColumnType("date");
            modelBuilder.Entity<dbProgram>().Property(ug => ug.Entry_User).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbProgram>().Property(ug => ug.Update_Date).HasColumnType("date");
            modelBuilder.Entity<dbProgram>().Property(ug => ug.Update_User).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbProgram>().Property(ug => ug.FLAG_AKTIF).HasColumnType("varchar(1)");

            //ModelMasterRekapTraining
            modelBuilder.Entity<dbRekapTraining>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbRekapTraining>().Property(ug => ug.TRN_ID).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbRekapTraining>().Property(ug => ug.Type).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbRekapTraining>().Property(ug => ug.Program).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbRekapTraining>().Property(ug => ug.ProgramTxt).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbRekapTraining>().Property(ug => ug.EDP).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbRekapTraining>().Property(ug => ug.Periode).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbRekapTraining>().Property(ug => ug.Week).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbRekapTraining>().Property(ug => ug.Date).HasColumnType("date");
            modelBuilder.Entity<dbRekapTraining>().Property(ug => ug.Participant).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbRekapTraining>().Property(ug => ug.Trainer).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbRekapTraining>().Property(ug => ug.idTrainer).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbRekapTraining>().Property(ug => ug.NoParticipant).HasColumnType("int");
            modelBuilder.Entity<dbRekapTraining>().Property(ug => ug.Days).HasColumnType("int");
            modelBuilder.Entity<dbRekapTraining>().Property(ug => ug.Hours).HasColumnType("decimal(6,2)");
            modelBuilder.Entity<dbRekapTraining>().Property(ug => ug.TotalHours).HasColumnType("int");
            modelBuilder.Entity<dbRekapTraining>().Property(ug => ug.Entry_Date).HasColumnType("date");
            modelBuilder.Entity<dbRekapTraining>().Property(ug => ug.Entry_User).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbRekapTraining>().Property(ug => ug.Update_Date).HasColumnType("date");
            modelBuilder.Entity<dbRekapTraining>().Property(ug => ug.Update_User).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbRekapTraining>().Property(ug => ug.FLAG_AKTIF).HasColumnType("varchar(1)");

            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.TRN_ID).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.Type).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.Program).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.ProgramTxt).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.EDP).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.Periode).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.Week).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.Date).HasColumnType("date");
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.Participant).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.Trainer).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.idTrainer).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.NoParticipant).HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.Days).HasColumnType("DECIMAL(6,2)");
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.Hours).HasColumnType("DECIMAL(6,2)");
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.TotalHours).HasColumnType("DECIMAL(6,2)");
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.Entry_Date).HasColumnType("date").IsRequired(false);
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.Entry_User).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.Update_Date).HasColumnType("date").IsRequired(false);
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.Update_User).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.FLAG_AKTIF).HasColumnType("varchar(1)");
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.TrainingDays).HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<dbrekapTrainingfixed>().Property(ug => ug.Batch).HasColumnType("int").IsRequired(false);

            //ModelMasterNilai
            modelBuilder.Entity<dbNilaiSS>().Property(ug => ug.ScoreId).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbNilaiSS>().Property(ug => ug.TRN_ID).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbNilaiSS>().Property(ug => ug.SS_CODE).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbNilaiSS>().Property(ug => ug.NILAI).HasColumnType("int");
            modelBuilder.Entity<dbNilaiSS>().Property(ug => ug.SERTIFIKAT).HasColumnType("varchar(1)");
            modelBuilder.Entity<dbNilaiSS>().Property(ug => ug.NoSertifikat).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbNilaiSS>().Property(ug => ug.FILE_SERTIFIKAT).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbNilaiSS>().Property(ug => ug.Entry_Date).HasColumnType("date");
            modelBuilder.Entity<dbNilaiSS>().Property(ug => ug.Entry_User).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbNilaiSS>().Property(ug => ug.Update_Date).HasColumnType("date");
            modelBuilder.Entity<dbNilaiSS>().Property(ug => ug.Update_User).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbNilaiSS>().Property(ug => ug.FLAG_AKTIF).HasColumnType("varchar(1)");

            modelBuilder.Entity<dbNilaiSSFixed>().Property(ug => ug.ScoreId).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbNilaiSSFixed>().Property(ug => ug.TRN_ID).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbNilaiSSFixed>().Property(ug => ug.SS_CODE).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbNilaiSSFixed>().Property(ug => ug.NILAI).HasColumnType("int");
            modelBuilder.Entity<dbNilaiSSFixed>().Property(ug => ug.SERTIFIKAT).HasColumnType("varchar(1)");
            modelBuilder.Entity<dbNilaiSSFixed>().Property(ug => ug.NoSertifikat).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbNilaiSSFixed>().Property(ug => ug.FILE_SERTIFIKAT).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbNilaiSSFixed>().Property(ug => ug.Entry_Date).HasColumnType("date");
            modelBuilder.Entity<dbNilaiSSFixed>().Property(ug => ug.Entry_User).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbNilaiSSFixed>().Property(ug => ug.Update_Date).HasColumnType("date");
            modelBuilder.Entity<dbNilaiSSFixed>().Property(ug => ug.Update_User).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbNilaiSSFixed>().Property(ug => ug.FLAG_AKTIF).HasColumnType("varchar(1)");
            modelBuilder.Entity<dbNilaiSSFixed>().Property(ug => ug.ISPRESENT).HasColumnType("varchar(1)");
            modelBuilder.Entity<dbNilaiSSFixed>().Property(ug => ug.EMP_TYPE).HasColumnType("varchar(255)");

            //SystemSlideTbl
            modelBuilder.Entity<dbSliderImg>().Property(ug => ug.ID).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbSliderImg>().Property(ug => ug.IMG_DESC).HasColumnType("int");
            modelBuilder.Entity<dbSliderImg>().Property(ug => ug.FILE_NAME).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbSliderImg>().Property(ug => ug.IMG_DESC).HasColumnType("MediumBlob").IsRequired(false);
            modelBuilder.Entity<dbSliderImg>().Property(ug => ug.FLAG_AKTIF).HasColumnType("varchar(1)").IsRequired(false);
            modelBuilder.Entity<dbSliderImg>().Property(ug => ug.ENTRY_USER).HasColumnType("varchar(50)").IsRequired(false);
            modelBuilder.Entity<dbSliderImg>().Property(ug => ug.UPDATE_USER).HasColumnType("varchar(10)").IsRequired();
            modelBuilder.Entity<dbSliderImg>().Property(ug => ug.ENTRY_DATE).HasColumnType("date");
            modelBuilder.Entity<dbSliderImg>().Property(ug => ug.UPDATE_DATE).HasColumnType("date");

            //trainerlisttbl
            modelBuilder.Entity<dbTrainerList>().Property(ug => ug.idTrainerList).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbTrainerList>().Property(ug => ug.idTrainer).HasColumnType("varchar(50)").IsRequired(false);
            modelBuilder.Entity<dbTrainerList>().Property(ug => ug.idFormRekap).HasColumnType("int");

            //CustomerTbl
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.EDP).HasColumnType("varchar(5)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.CUST_NAME).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.COMPANY).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.NPWP).HasColumnType("varchar(25)").IsRequired(false);
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.address).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.city).HasColumnType("varchar(80)").IsRequired(false);
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.province).HasColumnType("varchar(80)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.postal).HasColumnType("varchar(10)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.BANK_NAME).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.BANK_NUMBER).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.BANK_BRANCH).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.BANK_COUNTRY).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.REG_DATE).HasColumnType("date");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.BL_FLAG).HasColumnType("varchar(1)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.ENTRY_DATE).HasColumnType("date");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.UPDATE_DATE).HasColumnType("date");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.ENTRY_USER).HasColumnType("varchar(80)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.UPDATE_USER).HasColumnType("varchar(80)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.FLAG_AKTIF).HasColumnType("varchar(1)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.Email).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.KTP).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.PHONE1).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.PHONE2).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.isApproved).HasColumnType("varchar(1)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.isApproved2).HasColumnType("varchar(1)");

            modelBuilder.Entity<dbCustomer>().Property(ug => ug.VA1).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.VA1NOTE).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.VA2).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.VA2NOTE).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.FILE_KTP).HasColumnType("MediumBlob");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.FILE_AKTA).HasColumnType("MediumBlob");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.FILE_REKENING).HasColumnType("MediumBlob");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.FILE_NPWP).HasColumnType("MediumBlob");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.FILE_TDP).HasColumnType("MediumBlob");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.FILE_SIUP).HasColumnType("MediumBlob");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.FILE_NIB).HasColumnType("MediumBlob");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.FILE_SPPKP).HasColumnType("MediumBlob");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.FILE_SKT).HasColumnType("MediumBlob");

            modelBuilder.Entity<dbCustomer>().Property(ug => ug.FILE_KTP_NAME).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.FILE_AKTA_NAME).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.FILE_REKENING_NAME).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.FILE_NPWP_NAME).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.FILE_TDP_NAME).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.FILE_SIUP_NAME).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.FILE_NIB_NAME).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.FILE_SPPKP_NAME).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.FILE_SKT_NAME).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.store_area).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbCustomer>().Property(ug => ug.discount_customer).HasColumnType("varchar(50)");


            //Store list tables
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.COMPANY_ID).HasColumnType("varchar(50)").IsRequired(false);
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.STORE_NAME).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.STORE_ADDRESS).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.STORE_CITY).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.STORE_PROVINCE).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.STORE_POSTAL).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.STORE_BANK_NAME).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.STORE_BANK_NUMBER).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.STORE_BANK_BRANCH).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.STORE_BANK_COUNTRY).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.STORE_REG_DATE).HasColumnType("date");
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.STORE_BL_FLAG).HasColumnType("varchar(1)").IsRequired(false);
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.ENTRY_DATE).HasColumnType("datetime");
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.UPDATE_DATE).HasColumnType("datetime");
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.ENTRY_USER).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.UPDATE_USER).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.FLAG_AKTIF).HasColumnType("varchar(1)").IsRequired(false);
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.FILE_PHOTO_NAME).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.FILE_PHOTO).HasColumnType("MediumBlob").IsRequired(false);
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.STORE_MANAGER_NAME).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.STORE_MANAGER_EMAIL).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.STORE_MANAGER_PHONE).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.STORE_EMAIL).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbStoreList>().Property(ug => ug.STORE_MANAGER_KTP).HasColumnType("varchar(255)").IsRequired(false);

            //Article Tables
            modelBuilder.Entity<dbArticle>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbArticle>().Property(ug => ug.Article).HasColumnType("varchar(7)");
            modelBuilder.Entity<dbArticle>().Property(ug => ug.FILE_IMG_NAME).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbArticle>().Property(ug => ug.ENTRY_DATE).HasColumnType("date");
            modelBuilder.Entity<dbArticle>().Property(ug => ug.UPDATE_DATE).HasColumnType("date");
            modelBuilder.Entity<dbArticle>().Property(ug => ug.ENTRY_USER).HasColumnType("varchar(80)");
            modelBuilder.Entity<dbArticle>().Property(ug => ug.UPDATE_USER).HasColumnType("varchar(80)");
            modelBuilder.Entity<dbArticle>().Property(ug => ug.FILE_IMG).HasColumnType("MediumBlob");
            modelBuilder.Entity<dbArticle>().Property(ug => ug.FLAG_AKTIF).HasColumnType("varchar(1)");

            //SalesWholeSaleTbl
            modelBuilder.Entity<dbSalesWholeSale>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbSalesWholeSale>().Property(ug => ug.EMP_CODE).HasColumnType("varchar(10)");
            modelBuilder.Entity<dbSalesWholeSale>().Property(ug => ug.ENTRY_DATE).HasColumnType("date");
            modelBuilder.Entity<dbSalesWholeSale>().Property(ug => ug.UPDATE_DATE).HasColumnType("date");
            modelBuilder.Entity<dbSalesWholeSale>().Property(ug => ug.ENTRY_USER).HasColumnType("varchar(80)");
            modelBuilder.Entity<dbSalesWholeSale>().Property(ug => ug.UPDATE_USER).HasColumnType("varchar(80)");

            //SalesOrderTbl
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.EMP_CODE).HasColumnType("varchar(25)");
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.TOTAL_ORDER).HasColumnType("decimal(15,2)");
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.TOTAL_QTY).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.STATUS).HasColumnType("varchar(10)");
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.APPROVAL_1).HasColumnType("varchar(10)");
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.APPROVAL_2).HasColumnType("varchar(10)");
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.ENTRY_DATE).HasColumnType("date");
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.UPDATE_DATE).HasColumnType("date");
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.ENTRY_USER).HasColumnType("varchar(80)");
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.UPDATE_USER).HasColumnType("varchar(80)");
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.FLAG_AKTIF).HasColumnType("varchar(1)");
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.ORDER_DATE).HasColumnType("datetime");
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.id_customer).IsRequired(false).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.INV_DISC).HasColumnType("decimal(15,2)");
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.FLAG_SENT).HasColumnType("varchar(10)");
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.IS_DISC_PERC).HasColumnType("varchar(10)");
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.INV_DISC_AMT).HasColumnType("decimal(15,2)");
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.SHIPPING_ADDRESS).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.APPROVAL_AR).HasColumnType("varchar(1)");
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.EMAIL).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbSalesOrder>().Property(ug => ug.picking_no).HasColumnType("varchar(20)");

            //SalesOrderTblTemp
            modelBuilder.Entity<dbSalesOrderTemp>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbSalesOrderTemp>().Property(ug => ug.EMP_CODE).HasColumnType("varchar(25)");
            modelBuilder.Entity<dbSalesOrderTemp>().Property(ug => ug.TOTAL_ORDER).HasColumnType("decimal(15,2)");
            modelBuilder.Entity<dbSalesOrderTemp>().Property(ug => ug.TOTAL_QTY).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderTemp>().Property(ug => ug.STATUS).HasColumnType("varchar(10)");
            modelBuilder.Entity<dbSalesOrderTemp>().Property(ug => ug.APPROVAL_1).HasColumnType("varchar(10)");
            modelBuilder.Entity<dbSalesOrderTemp>().Property(ug => ug.APPROVAL_2).HasColumnType("varchar(10)");
            modelBuilder.Entity<dbSalesOrderTemp>().Property(ug => ug.ENTRY_DATE).HasColumnType("date");
            modelBuilder.Entity<dbSalesOrderTemp>().Property(ug => ug.UPDATE_DATE).HasColumnType("date");
            modelBuilder.Entity<dbSalesOrderTemp>().Property(ug => ug.ENTRY_USER).HasColumnType("varchar(80)");
            modelBuilder.Entity<dbSalesOrderTemp>().Property(ug => ug.UPDATE_USER).HasColumnType("varchar(80)");
            modelBuilder.Entity<dbSalesOrderTemp>().Property(ug => ug.FLAG_AKTIF).HasColumnType("varchar(1)");
            modelBuilder.Entity<dbSalesOrderTemp>().Property(ug => ug.ORDER_DATE).HasColumnType("datetime");
            modelBuilder.Entity<dbSalesOrderTemp>().Property(ug => ug.id_customer).IsRequired(false).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderTemp>().Property(ug => ug.INV_DISC).HasColumnType("decimal(15,2)");
            modelBuilder.Entity<dbSalesOrderTemp>().Property(ug => ug.FLAG_SENT).HasColumnType("varchar(10)");
            modelBuilder.Entity<dbSalesOrderTemp>().Property(ug => ug.IS_DISC_PERC).HasColumnType("varchar(10)");
            modelBuilder.Entity<dbSalesOrderTemp>().Property(ug => ug.INV_DISC_AMT).HasColumnType("decimal(15,2)");
            modelBuilder.Entity<dbSalesOrderTemp>().Property(ug => ug.SHIPPING_ADDRESS).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbSalesOrderTemp>().Property(ug => ug.APPROVAL_AR).HasColumnType("varchar(1)");
            modelBuilder.Entity<dbSalesOrderTemp>().Property(ug => ug.EMAIL).HasColumnType("varchar(255)");


            //SalesOrderDtlTbl
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.id_order).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.article).HasColumnType("varchar(10)");
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.size).HasColumnType("varchar(10)");
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.qty).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.price).HasColumnType("decimal(15,2)");
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.disc).HasColumnType("decimal(15,2)");
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.final_price).HasColumnType("decimal(15,2)");
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.Size_1).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.Size_2).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.Size_3).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.Size_4).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.Size_5).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.Size_6).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.Size_7).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.Size_8).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.Size_9).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.Size_10).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.Size_11).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.Size_12).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.Size_13).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.disc_amt).HasColumnType("decimal(15,2)");
            modelBuilder.Entity<dbSalesOrderDtl>().Property(ug => ug.is_disc_perc).HasColumnType("varchar(10)");

            //SalesOrderDtlTblTemp
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.id_order).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.article).HasColumnType("varchar(10)");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.size).HasColumnType("varchar(10)");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.qty).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.price).HasColumnType("decimal(15,2)");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.disc).HasColumnType("decimal(15,2)");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.final_price).HasColumnType("decimal(15,2)");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.Size_1).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.Size_2).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.Size_3).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.Size_4).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.Size_5).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.Size_6).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.Size_7).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.Size_8).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.Size_9).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.Size_10).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.Size_11).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.Size_12).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.Size_13).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.disc_amt).HasColumnType("decimal(15,2)");
            modelBuilder.Entity<dbSalesOrderDtlTemp>().Property(ug => ug.is_disc_perc).HasColumnType("varchar(10)");


            //SalesOrderDtlTblCredit
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.id_order).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.article).HasColumnType("varchar(10)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.size).HasColumnType("varchar(10)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.qty).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.price).HasColumnType("decimal(15,2)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.disc).HasColumnType("decimal(15,2)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.final_price).HasColumnType("decimal(15,2)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.Size_1).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.Size_2).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.Size_3).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.Size_4).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.Size_5).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.Size_6).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.Size_7).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.Size_8).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.Size_9).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.Size_10).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.Size_11).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.Size_12).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.Size_13).HasColumnType("int(10)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.disc_amt).HasColumnType("decimal(15,2)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.is_disc_perc).HasColumnType("varchar(10)");
            modelBuilder.Entity<dbSalesOrderDtlCredit>().Property(ug => ug.picking_no).HasColumnType("varchar(20)");

            //PaymentListTbl
            modelBuilder.Entity<dbPaymentList>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbPaymentList>().Property(ug => ug.id_order).HasColumnType("int(10)");
            modelBuilder.Entity<dbPaymentList>().Property(ug => ug.id_customer).HasColumnType("int(10)");

            modelBuilder.Entity<dbPaymentList>().Property(ug => ug.BANK).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbPaymentList>().Property(ug => ug.REF_ID).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbPaymentList>().Property(ug => ug.FILE_PAYMENT).HasColumnType("MediumBlob");
            modelBuilder.Entity<dbPaymentList>().Property(ug => ug.TOTAL_PAY).HasColumnType("decimal(15,2)");
            modelBuilder.Entity<dbPaymentList>().Property(ug => ug.ENTRY_DATE).HasColumnType("date");
            modelBuilder.Entity<dbPaymentList>().Property(ug => ug.UPDATE_DATE).HasColumnType("date");
            modelBuilder.Entity<dbPaymentList>().Property(ug => ug.ENTRY_USER).HasColumnType("varchar(80)");
            modelBuilder.Entity<dbPaymentList>().Property(ug => ug.UPDATE_USER).HasColumnType("varchar(80)");
            modelBuilder.Entity<dbPaymentList>().Property(ug => ug.FLAG_AKTIF).HasColumnType("varchar(1)");
            modelBuilder.Entity<dbPaymentList>().Property(ug => ug.PAYMENT_DATE).HasColumnType("datetime");
            modelBuilder.Entity<dbPaymentList>().Property(ug => ug.FILE_PAYMENT_NAME).HasColumnType("varchar(255)");

            //dborderconfig
            modelBuilder.Entity<dbOrderConfig>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbOrderConfig>().Property(ug => ug.pairsarticle).HasColumnType("int");
            modelBuilder.Entity<dbOrderConfig>().Property(ug => ug.pairsorder).HasColumnType("int");
            modelBuilder.Entity<dbOrderConfig>().Property(ug => ug.FLAG_AKTIF).HasColumnType("varchar(1)").IsRequired(false);
            modelBuilder.Entity<dbOrderConfig>().Property(ug => ug.ENTRY_USER).HasColumnType("varchar(50)").IsRequired(false);
            modelBuilder.Entity<dbOrderConfig>().Property(ug => ug.UPDATE_USER).HasColumnType("varchar(10)").IsRequired();
            modelBuilder.Entity<dbOrderConfig>().Property(ug => ug.ENTRY_DATE).HasColumnType("date");
            modelBuilder.Entity<dbOrderConfig>().Property(ug => ug.UPDATE_DATE).HasColumnType("date");

            //dbsalescreditlog
            modelBuilder.Entity<dbsalescreditlog>().Property(ug => ug.invno).HasColumnType("varchar(50)").IsRequired(true);
            modelBuilder.Entity<dbsalescreditlog>().Property(ug => ug.year).HasColumnType("int");
            modelBuilder.Entity<dbsalescreditlog>().Property(ug => ug.valuecredit).HasColumnType("decimal(15,2)");
            modelBuilder.Entity<dbsalescreditlog>().Property(ug => ug.isused).HasColumnType("varchar(1)").IsRequired(false);
            modelBuilder.Entity<dbsalescreditlog>().Property(ug => ug.edp).HasColumnType("varchar(15)").IsRequired(true);



            //modelBuilder.Entity<VaksinModel>().Property(ug => ug.CreationDateTime).HasColumnType("datetime").IsRequired();  
            //modelBuilder.Entity<VaksinModel>().Property(ug => ug.LastUpdateDateTime).HasColumnType("datetime").IsRequired(false);  

            //modelBuilder.Entity<User>().Property(u => u.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            //modelBuilder.Entity<User>().Property(u => u.FirstName).HasColumnType("nvarchar(50)").IsRequired();  
            //modelBuilder.Entity<User>().Property(u => u.LastName).HasColumnType("nvarchar(50)").IsRequired();  
            //modelBuilder.Entity<User>().Property(u => u.UserGroupId).HasColumnType("int").IsRequired();  
            //modelBuilder.Entity<User>().Property(u => u.CreationDateTime).HasColumnType("datetime").IsRequired();  
            //modelBuilder.Entity<User>().Property(u => u.LastUpdateDateTime).HasColumnType("datetime").IsRequired(false);  

            // Configure relationships  
            //modelBuilder.Entity<VaksinModel>().HasOne<XstoreModel>().WithMany().HasPrincipalKey(ug => ug.edp).HasForeignKey(u => u.EDP_CODE).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_xStoredata_orcl");
            //modelBuilder.Entity<dbRekapTraining>().HasOne<dbTrainer>().WithMany().HasPrincipalKey(ug => ug.idTrainer).HasForeignKey(u => u.idTrainer).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_idTrainerdt");
            //modelBuilder.Entity<dbRekapTraining>().HasOne<XstoreModel>().WithMany().HasPrincipalKey(ug => ug.edp).HasForeignKey(u => u.EDP).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_xStoredata_orcl_2");

        }
    }
}
