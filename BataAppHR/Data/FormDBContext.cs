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
        public DbSet<SystemTabModel> TabTbl { get; set; }
        public DbSet<SystemMenuModel> MenuTbl { get; set; }
        public DbSet<dbSliderImg> SlideTbl { get; set; }
        public DbSet<dbCustomer> CustomerTbl { get; set; }

        public DbSet<dbStoreList> StoreListTbl { get; set; }
        public DbSet<dbItemMaster> ItemMasterTbl { get; set; }
        public DbSet<dbCategory> CategoryTbl { get; set; }
        public DbSet<dbSubCategory> SubCategoryTbl { get; set; }
        public DbSet<dbItemStore> ItemStoreTbl { get; set; }
        public DbSet<dbSalesStaff> SalesStaffTbl { get; set; }
        public DbSet<dbStoreStockModel> StockTbl { get; set; }
        public DbSet<dbDiscount> DiscTbl { get; set; }
        public DbSet<dbDiscountStoreList> StoreDiscTbl { get; set; }

        public DbSet<dbSalesHdr> saleshdrtbl { get; set; }
        public DbSet<dbSalesDtl> salesdtltbl { get; set; }


        public FormDBContext(DbContextOptions<FormDBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use Fluent API to configure  

            // Map entities to tables  
         
            modelBuilder.Entity<dbCompanySeq>().ToTable("dbCompany_seq");

            modelBuilder.Entity<dbCustomerSeq>().ToTable("dbCustomer_seq");

            modelBuilder.Entity<SystemTabModel>().ToTable("SystemTabTbl");
            modelBuilder.Entity<SystemMenuModel>().ToTable("SystemMenuTbl");
            modelBuilder.Entity<dbSliderImg>().ToTable("dbSliderImg");

            modelBuilder.Entity<dbCustomer>().ToTable("dbCustomer");
            modelBuilder.Entity<dbStoreList>().ToTable("dbStoreList");
            modelBuilder.Entity<dbItemMaster>().ToTable("dbItemMaster");
            modelBuilder.Entity<dbCategory>().ToTable("dbCategory");
            modelBuilder.Entity<dbSubCategory>().ToTable("dbSubCategory");
            modelBuilder.Entity<dbItemStore>().ToTable("dbItemStore");
            modelBuilder.Entity<dbSalesStaff>().ToTable("dbSalesStaff");
            modelBuilder.Entity<dbStoreStockModel>().ToTable("dbStoreStock");
            modelBuilder.Entity<dbDiscount>().ToTable("dbDiscount");
            modelBuilder.Entity<dbSalesHdr>().ToTable("dbSalesHdr").HasNoKey();
            modelBuilder.Entity<dbSalesDtl>().ToTable("dbSalesDtl").HasNoKey();
            modelBuilder.Entity<dbDiscountStoreList>().ToTable("dbDiscountStoreList").HasNoKey();


            modelBuilder.Entity<SystemTabModel>().HasKey(ug => ug.ID).HasName("PK_TAB_Seq");
            modelBuilder.Entity<SystemMenuModel>().HasKey(ug => ug.ID).HasName("PK_MENU_Seq");
            modelBuilder.Entity<dbCompanySeq>().HasKey(ug => ug.id).HasName("PK_Company_Seq");

            modelBuilder.Entity<dbSliderImg>().HasKey(ug => ug.ID).HasName("PK_SliderImg");
            modelBuilder.Entity<dbCustomer>().HasKey(ug => ug.id).HasName("PK_Customer");
            modelBuilder.Entity<dbCustomerSeq>().HasKey(ug => ug.id).HasName("PK_Cust_Seq");
            modelBuilder.Entity<dbStoreList>().HasKey(ug => ug.id).HasName("PK_StoreList");
            modelBuilder.Entity<dbItemMaster>().HasKey(ug => ug.id).HasName("PK_ItemMaster");
            modelBuilder.Entity<dbCategory>().HasKey(ug => ug.id).HasName("PK_Category");
            modelBuilder.Entity<dbSubCategory>().HasKey(ug => ug.id).HasName("PK_SubCategory");
            modelBuilder.Entity<dbItemStore>().HasKey(ug => ug.id).HasName("PK_ItemStore");
            modelBuilder.Entity<dbSalesStaff>().HasKey(ug => ug.id).HasName("PK_SalesStaff");
            modelBuilder.Entity<dbDiscount>().HasKey(ug => ug.id).HasName("PK_Discount");

            // Configure indexes  
            //modelBuilder.Entity<UserGroup>().HasIndex(p => p.Name).IsUnique().HasDatabaseName("Idx_Name");  
            //modelBuilder.Entity<User>().HasIndex(u => u.FirstName).HasDatabaseName("Idx_FirstName");  
            //modelBuilder.Entity<User>().HasIndex(u => u.LastName).HasDatabaseName("Idx_LastName");  


            //Model Sequence
            modelBuilder.Entity<dbCustomerSeq>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbCompanySeq>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();

           
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

            #region onposmodel
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


            //Sales Staff tables
            modelBuilder.Entity<dbSalesStaff>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbSalesStaff>().Property(ug => ug.COMPANY_ID).HasColumnType("varchar(50)").IsRequired(false);
            modelBuilder.Entity<dbSalesStaff>().Property(ug => ug.STORE_ID).HasColumnType("int");
            modelBuilder.Entity<dbSalesStaff>().Property(ug => ug.SALES_NAME).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbSalesStaff>().Property(ug => ug.SALES_REG_DATE).HasColumnType("date");
            modelBuilder.Entity<dbSalesStaff>().Property(ug => ug.ENTRY_DATE).HasColumnType("datetime");
            modelBuilder.Entity<dbSalesStaff>().Property(ug => ug.UPDATE_DATE).HasColumnType("datetime");
            modelBuilder.Entity<dbSalesStaff>().Property(ug => ug.ENTRY_USER).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbSalesStaff>().Property(ug => ug.UPDATE_USER).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbSalesStaff>().Property(ug => ug.FLAG_AKTIF).HasColumnType("varchar(1)").IsRequired(false);
            modelBuilder.Entity<dbSalesStaff>().Property(ug => ug.FILE_PHOTO_NAME).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbSalesStaff>().Property(ug => ug.FILE_PHOTO).HasColumnType("MediumBlob").IsRequired(false);
            modelBuilder.Entity<dbSalesStaff>().Property(ug => ug.SALES_PHONE).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbSalesStaff>().Property(ug => ug.SALES_EMAIL).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbSalesStaff>().Property(ug => ug.SALES_KTP).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbSalesStaff>().Property(ug => ug.SALES_BLACKLIST_FLAG).HasColumnType("varchar(1)").IsRequired(false);



            //dbItemMaster
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.COMPANY_ID).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.itemid).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.color).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.size).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.category).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.subcategory).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.itemdescription).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.price1).HasColumnType("decimal(19,2)");
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.price2).HasColumnType("decimal(19,2)");
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.price3).HasColumnType("decimal(19,2)");
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.brand).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.FLAG_AKTIF).HasColumnType("varchar(1)").IsRequired(false);
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.ENTRY_USER).HasColumnType("varchar(50)").IsRequired(false);
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.UPDATE_USER).HasColumnType("varchar(10)").IsRequired();
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.ENTRY_DATE).HasColumnType("date");
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.UPDATE_DATE).HasColumnType("date");
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.CY_amount).HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.CY_qty).HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.LY_amount).HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.LY_qty).HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.month_age).HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.month_amount).HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.month_qty).HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<dbItemMaster>().Property(ug => ug.year_age).HasColumnType("int").IsRequired(false);


            //dbCategory
            modelBuilder.Entity<dbCategory>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbCategory>().Property(ug => ug.COMPANY_ID).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbCategory>().Property(ug => ug.Category).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbCategory>().Property(ug => ug.description).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbCategory>().Property(ug => ug.FLAG_AKTIF).HasColumnType("varchar(1)").IsRequired(false);
            modelBuilder.Entity<dbCategory>().Property(ug => ug.ENTRY_USER).HasColumnType("varchar(50)").IsRequired(false);
            modelBuilder.Entity<dbCategory>().Property(ug => ug.UPDATE_USER).HasColumnType("varchar(10)").IsRequired();
            modelBuilder.Entity<dbCategory>().Property(ug => ug.ENTRY_DATE).HasColumnType("date");
            modelBuilder.Entity<dbCategory>().Property(ug => ug.UPDATE_DATE).HasColumnType("date");

            //dbsubcategory
            modelBuilder.Entity<dbSubCategory>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbSubCategory>().Property(ug => ug.COMPANY_ID).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbSubCategory>().Property(ug => ug.Category).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbSubCategory>().Property(ug => ug.SubCategory).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbSubCategory>().Property(ug => ug.description).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbSubCategory>().Property(ug => ug.FLAG_AKTIF).HasColumnType("varchar(1)").IsRequired(false);
            modelBuilder.Entity<dbSubCategory>().Property(ug => ug.ENTRY_USER).HasColumnType("varchar(50)").IsRequired(false);
            modelBuilder.Entity<dbSubCategory>().Property(ug => ug.UPDATE_USER).HasColumnType("varchar(10)").IsRequired();
            modelBuilder.Entity<dbSubCategory>().Property(ug => ug.ENTRY_DATE).HasColumnType("date");
            modelBuilder.Entity<dbSubCategory>().Property(ug => ug.UPDATE_DATE).HasColumnType("date");

            //dbitemstore
            modelBuilder.Entity<dbItemStore>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbItemStore>().Property(ug => ug.COMPANY_ID).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbItemStore>().Property(ug => ug.storeid).HasColumnType("int");
            modelBuilder.Entity<dbItemStore>().Property(ug => ug.itemidint).HasColumnType("int");
            modelBuilder.Entity<dbItemStore>().Property(ug => ug.STORE_NAME).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbItemStore>().Property(ug => ug.itemid).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbItemStore>().Property(ug => ug.FLAG_AKTIF).HasColumnType("varchar(1)").IsRequired(false);
            modelBuilder.Entity<dbItemStore>().Property(ug => ug.ENTRY_USER).HasColumnType("varchar(50)").IsRequired(false);
            modelBuilder.Entity<dbItemStore>().Property(ug => ug.UPDATE_USER).HasColumnType("varchar(10)").IsRequired();
            modelBuilder.Entity<dbItemStore>().Property(ug => ug.ENTRY_DATE).HasColumnType("date");
            modelBuilder.Entity<dbItemStore>().Property(ug => ug.UPDATE_DATE).HasColumnType("date");


            //dbStoreStock
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.stock_id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.storeid).HasColumnType("int");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.itemid).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.itmname).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.cat).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.subcat).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.area).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.row).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.rack).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.racklvl).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.bin_id).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.Past_stock).HasColumnType("int");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.dispatch_qty).HasColumnType("int");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.sales_qty).HasColumnType("int");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.receive_qty).HasColumnType("int");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.Current_stock).HasColumnType("int");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.s33).HasColumnType("int");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.s34).HasColumnType("int");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.s35).HasColumnType("int");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.s36).HasColumnType("int");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.s37).HasColumnType("int");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.s38).HasColumnType("int");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.s39).HasColumnType("int");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.s40).HasColumnType("int");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.s41).HasColumnType("int");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.s42).HasColumnType("int");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.s43).HasColumnType("int");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.s44).HasColumnType("int");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.s45).HasColumnType("int");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.s46).HasColumnType("int");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.standard_qty).HasColumnType("int");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.lastoutdate).HasColumnType("datetime");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.lastrcvdate).HasColumnType("datetime");
            modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.bucket_id).HasColumnType("varchar(100)").IsRequired(false);

            //Discount tables
            modelBuilder.Entity<dbDiscount>().Property(ug => ug.id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<dbDiscount>().Property(ug => ug.article).HasColumnType("varchar(50)").IsRequired();
            modelBuilder.Entity<dbDiscount>().Property(ug => ug.type).HasColumnType("varchar(255)");
            modelBuilder.Entity<dbDiscount>().Property(ug => ug.isallstore).HasColumnType("varchar(1)");
            modelBuilder.Entity<dbDiscount>().Property(ug => ug.COMPANY_ID).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbDiscount>().Property(ug => ug.promo_name).HasColumnType("varchar(255)");

            modelBuilder.Entity<dbDiscount>().Property(ug => ug.percentage).HasColumnType("int");
            modelBuilder.Entity<dbDiscount>().Property(ug => ug.amount).HasColumnType("int");
            modelBuilder.Entity<dbDiscount>().Property(ug => ug.entry_date).HasColumnType("datetime");
            modelBuilder.Entity<dbDiscount>().Property(ug => ug.update_date).HasColumnType("datetime");
            modelBuilder.Entity<dbDiscount>().Property(ug => ug.validfrom).HasColumnType("datetime");
            modelBuilder.Entity<dbDiscount>().Property(ug => ug.validto).HasColumnType("datetime");
            modelBuilder.Entity<dbDiscount>().Property(ug => ug.status).HasColumnType("varchar(1)");
            modelBuilder.Entity<dbDiscount>().Property(ug => ug.entry_user).HasColumnType("varchar(255)").IsRequired(false);
            modelBuilder.Entity<dbDiscount>().Property(ug => ug.update_user).HasColumnType("varchar(255)").IsRequired(false);

            //Store Discount Tables
            modelBuilder.Entity<dbDiscountStoreList>().Property(ug => ug.storeid).HasColumnType("int");
            modelBuilder.Entity<dbDiscountStoreList>().Property(ug => ug.COMPANY_ID).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbDiscountStoreList>().Property(ug => ug.promoid).HasColumnType("int").IsRequired();

            //saleshdr
            modelBuilder.Entity<dbSalesHdr>().Property(ug => ug.invoice).HasColumnType("varchar(50)").IsRequired();
            modelBuilder.Entity<dbSalesHdr>().Property(ug => ug.Store_id).HasColumnType("int");
            modelBuilder.Entity<dbSalesHdr>().Property(ug => ug.staff_id).HasColumnType("int");
            modelBuilder.Entity<dbSalesHdr>().Property(ug => ug.transdate).HasColumnType("datetime");
            modelBuilder.Entity<dbSalesHdr>().Property(ug => ug.trans_amount).HasColumnType("int");
            modelBuilder.Entity<dbSalesHdr>().Property(ug => ug.trans_qty).HasColumnType("int");
            modelBuilder.Entity<dbSalesHdr>().Property(ug => ug.approval_code).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbSalesHdr>().Property(ug => ug.cardnum).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbSalesHdr>().Property(ug => ug.transtype).HasColumnType("varchar(100)");
            modelBuilder.Entity<dbSalesHdr>().Property(ug => ug.update_date).HasColumnType("datetime");
            modelBuilder.Entity<dbSalesHdr>().Property(ug => ug.update_user).HasColumnType("varchar(255)");

            //salesdtl
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.invoice).HasColumnType("varchar(50)").IsRequired();
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.store_id).HasColumnType("int");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.transdate).HasColumnType("datetime");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.article).HasColumnType("varchar(50)").IsRequired();
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.cat).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.subcat).HasColumnType("varchar(50)");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.price).HasColumnType("int");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.discountcode).HasColumnType("int");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.disc_amount).HasColumnType("int");

            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.disc_amount).HasColumnType("int");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.disc_prc).HasColumnType("int");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.qty).HasColumnType("int");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.s33).HasColumnType("int");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.s34).HasColumnType("int");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.s35).HasColumnType("int");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.s36).HasColumnType("int");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.s37).HasColumnType("int");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.s38).HasColumnType("int");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.s39).HasColumnType("int");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.s40).HasColumnType("int");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.s41).HasColumnType("int");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.s42).HasColumnType("int");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.s43).HasColumnType("int");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.s44).HasColumnType("int");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.s45).HasColumnType("int");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.s46).HasColumnType("int");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.update_date).HasColumnType("datetime");
            modelBuilder.Entity<dbSalesDtl>().Property(ug => ug.update_user).HasColumnType("varchar(255)");


            //modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.ENTRY_USER).HasColumnType("varchar(50)").IsRequired(false);
            //modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.UPDATE_USER).HasColumnType("varchar(10)").IsRequired();
            //modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.ENTRY_DATE).HasColumnType("date");
            //modelBuilder.Entity<dbStoreStockModel>().Property(ug => ug.UPDATE_DATE).HasColumnType("date");
            #endregion onposmodel



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
