﻿// <auto-generated />
using System;
using BataAppHR.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BataAppHR.Migrations.FormDB
{
    [DbContext(typeof(FormDBContext))]
    partial class FormDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("BataAppHR.Models.SystemMenuModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ENTRY_DATE")
                        .HasColumnType("date");

                    b.Property<string>("ENTRY_USER")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("FLAG_AKTIF")
                        .HasColumnType("varchar(1)");

                    b.Property<string>("MENU_DESC")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("MENU_LINK")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("MENU_TXT")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ROLE_ID")
                        .HasColumnType("varchar(75)");

                    b.Property<int>("TAB_ID")
                        .HasColumnType("int");

                    b.Property<DateTime>("UPDATE_DATE")
                        .HasColumnType("date");

                    b.Property<string>("UPDATE_USER")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.HasKey("ID")
                        .HasName("PK_MENU_Seq");

                    b.ToTable("SystemMenuTbl");
                });

            modelBuilder.Entity("BataAppHR.Models.SystemTabModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ENTRY_DATE")
                        .HasColumnType("date");

                    b.Property<string>("ENTRY_USER")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("FLAG_AKTIF")
                        .HasColumnType("varchar(1)");

                    b.Property<string>("ROLE_ID")
                        .HasColumnType("varchar(75)");

                    b.Property<string>("TAB_DESC")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("TAB_TXT")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("UPDATE_DATE")
                        .HasColumnType("date");

                    b.Property<string>("UPDATE_USER")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.HasKey("ID")
                        .HasName("PK_TAB_Seq");

                    b.ToTable("SystemTabTbl");
                });

            modelBuilder.Entity("BataAppHR.Models.dbCustomer", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BANK_BRANCH")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("BANK_COUNTRY")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("BANK_NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("BANK_NUMBER")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("BL_FLAG")
                        .HasColumnType("varchar(1)");

                    b.Property<string>("COMPANY")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("COMPANY_ID")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("CUST_NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("EDP")
                        .HasColumnType("varchar(5)");

                    b.Property<DateTime>("ENTRY_DATE")
                        .HasColumnType("date");

                    b.Property<string>("ENTRY_USER")
                        .HasColumnType("varchar(80)");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(100)");

                    b.Property<byte[]>("FILE_AKTA")
                        .HasColumnType("MediumBlob");

                    b.Property<string>("FILE_AKTA_NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<byte[]>("FILE_KTP")
                        .HasColumnType("MediumBlob");

                    b.Property<string>("FILE_KTP_NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<byte[]>("FILE_NIB")
                        .HasColumnType("MediumBlob");

                    b.Property<string>("FILE_NIB_NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<byte[]>("FILE_NPWP")
                        .HasColumnType("MediumBlob");

                    b.Property<string>("FILE_NPWP_NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<byte[]>("FILE_REKENING")
                        .HasColumnType("MediumBlob");

                    b.Property<string>("FILE_REKENING_NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<byte[]>("FILE_SIUP")
                        .HasColumnType("MediumBlob");

                    b.Property<string>("FILE_SIUP_NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<byte[]>("FILE_SKT")
                        .HasColumnType("MediumBlob");

                    b.Property<string>("FILE_SKT_NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<byte[]>("FILE_SPPKP")
                        .HasColumnType("MediumBlob");

                    b.Property<string>("FILE_SPPKP_NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<byte[]>("FILE_TDP")
                        .HasColumnType("MediumBlob");

                    b.Property<string>("FILE_TDP_NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FLAG_AKTIF")
                        .HasColumnType("varchar(1)");

                    b.Property<string>("KTP")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("NPWP")
                        .HasColumnType("varchar(25)");

                    b.Property<string>("PHONE1")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("PHONE2")
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("REG_DATE")
                        .HasColumnType("date");

                    b.Property<DateTime>("UPDATE_DATE")
                        .HasColumnType("date");

                    b.Property<string>("UPDATE_USER")
                        .HasColumnType("varchar(80)");

                    b.Property<string>("VA1")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("VA1NOTE")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("VA2")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("VA2NOTE")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("address")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("city")
                        .HasColumnType("varchar(80)");

                    b.Property<string>("discount_customer")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("isApproved")
                        .HasColumnType("varchar(1)");

                    b.Property<string>("isApproved2")
                        .HasColumnType("varchar(1)");

                    b.Property<string>("postal")
                        .HasColumnType("varchar(10)");

                    b.Property<string>("province")
                        .HasColumnType("varchar(80)");

                    b.Property<string>("store_area")
                        .HasColumnType("varchar(50)");

                    b.Property<int>("totalstoreconfig")
                        .HasColumnType("int");

                    b.HasKey("id")
                        .HasName("PK_Customer");

                    b.ToTable("dbCustomer");
                });

            modelBuilder.Entity("BataAppHR.Models.dbCustomerSeq", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.HasKey("id")
                        .HasName("PK_Cust_Seq");

                    b.ToTable("dbCustomer_seq");
                });

            modelBuilder.Entity("BataAppHR.Models.dbSliderImg", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ENTRY_DATE")
                        .HasColumnType("date");

                    b.Property<string>("ENTRY_USER")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("FILE_NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FLAG_AKTIF")
                        .HasColumnType("varchar(1)");

                    b.Property<byte[]>("IMG_DESC")
                        .HasColumnType("MediumBlob");

                    b.Property<byte[]>("SLIDE_IMG_BLOB")
                        .HasColumnType("longblob");

                    b.Property<DateTime>("UPDATE_DATE")
                        .HasColumnType("date");

                    b.Property<string>("UPDATE_USER")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.HasKey("ID")
                        .HasName("PK_SliderImg");

                    b.ToTable("dbSliderImg");
                });

            modelBuilder.Entity("OnPOS.Models.dbCategory", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("COMPANY_ID")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Category")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("ENTRY_DATE")
                        .HasColumnType("date");

                    b.Property<string>("ENTRY_USER")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("FLAG_AKTIF")
                        .HasColumnType("varchar(1)");

                    b.Property<DateTime>("UPDATE_DATE")
                        .HasColumnType("date");

                    b.Property<string>("UPDATE_USER")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("description")
                        .HasColumnType("varchar(100)");

                    b.HasKey("id")
                        .HasName("PK_Category");

                    b.ToTable("dbCategory");
                });

            modelBuilder.Entity("OnPOS.Models.dbCompanySeq", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.HasKey("id")
                        .HasName("PK_Company_Seq");

                    b.ToTable("dbCompany_seq");
                });

            modelBuilder.Entity("OnPOS.Models.dbDepartment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("COMPANY_ID")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("CodeDivisi")
                        .HasColumnType("varchar(2)");

                    b.Property<string>("Color")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("DivisiName")
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("ENTRY_DATE")
                        .HasColumnType("date");

                    b.Property<string>("ENTRY_USER")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("FLAG_AKTIF")
                        .HasColumnType("varchar(1)");

                    b.Property<DateTime>("UPDATE_DATE")
                        .HasColumnType("date");

                    b.Property<string>("UPDATE_USER")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("codemaincat")
                        .HasColumnType("varchar(2)");

                    b.Property<string>("codesubcat")
                        .HasColumnType("varchar(2)");

                    b.Property<string>("colorcode")
                        .HasColumnType("varchar(2)");

                    b.Property<string>("gender")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("gendercode")
                        .HasColumnType("varchar(2)");

                    b.Property<string>("maincat")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("subcat")
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id")
                        .HasName("PK_Department");

                    b.ToTable("dbDepartment");
                });

            modelBuilder.Entity("OnPOS.Models.dbDiscount", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("COMPANY_ID")
                        .HasColumnType("varchar(100)");

                    b.Property<int>("amount")
                        .HasColumnType("int");

                    b.Property<string>("article")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("entry_date")
                        .HasColumnType("datetime");

                    b.Property<string>("entry_user")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("isallstore")
                        .HasColumnType("varchar(1)");

                    b.Property<int>("percentage")
                        .HasColumnType("int");

                    b.Property<string>("promo_name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("status")
                        .HasColumnType("varchar(1)");

                    b.Property<string>("type")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("update_date")
                        .HasColumnType("datetime");

                    b.Property<string>("update_user")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("validfrom")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("validto")
                        .HasColumnType("datetime");

                    b.HasKey("id")
                        .HasName("PK_Discount");

                    b.ToTable("dbDiscount");
                });

            modelBuilder.Entity("OnPOS.Models.dbDiscountStoreList", b =>
                {
                    b.Property<string>("COMPANY_ID")
                        .HasColumnType("varchar(100)");

                    b.Property<int>("promoid")
                        .HasColumnType("int");

                    b.Property<int>("storeid")
                        .HasColumnType("int");

                    b.ToTable("dbDiscountStoreList");
                });

            modelBuilder.Entity("OnPOS.Models.dbItemMaster", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("COMPANY_ID")
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("CY_amount")
                        .HasColumnType("int");

                    b.Property<int?>("CY_qty")
                        .HasColumnType("int");

                    b.Property<DateTime>("ENTRY_DATE")
                        .HasColumnType("date");

                    b.Property<string>("ENTRY_USER")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("FLAG_AKTIF")
                        .HasColumnType("varchar(1)");

                    b.Property<int?>("LY_amount")
                        .HasColumnType("int");

                    b.Property<int?>("LY_qty")
                        .HasColumnType("int");

                    b.Property<DateTime>("UPDATE_DATE")
                        .HasColumnType("date");

                    b.Property<string>("UPDATE_USER")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("brand")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("category")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("codedivisi")
                        .HasColumnType("varchar(2)");

                    b.Property<string>("color")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("itemdescription")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("itemid")
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("month_age")
                        .HasColumnType("int");

                    b.Property<int?>("month_amount")
                        .HasColumnType("int");

                    b.Property<int?>("month_qty")
                        .HasColumnType("int");

                    b.Property<decimal>("price1")
                        .HasColumnType("decimal(19,2)");

                    b.Property<decimal>("price2")
                        .HasColumnType("decimal(19,2)");

                    b.Property<decimal>("price3")
                        .HasColumnType("decimal(19,2)");

                    b.Property<string>("size")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("subcategory")
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("year_age")
                        .HasColumnType("int");

                    b.HasKey("id")
                        .HasName("PK_ItemMaster");

                    b.ToTable("dbItemMaster");
                });

            modelBuilder.Entity("OnPOS.Models.dbItemStore", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("COMPANY_ID")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("ENTRY_DATE")
                        .HasColumnType("date");

                    b.Property<string>("ENTRY_USER")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("FLAG_AKTIF")
                        .HasColumnType("varchar(1)");

                    b.Property<string>("STORE_NAME")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("UPDATE_DATE")
                        .HasColumnType("date");

                    b.Property<string>("UPDATE_USER")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("itemid")
                        .HasColumnType("varchar(50)");

                    b.Property<int>("itemidint")
                        .HasColumnType("int");

                    b.Property<int>("storeid")
                        .HasColumnType("int");

                    b.HasKey("id")
                        .HasName("PK_ItemStore");

                    b.ToTable("dbItemStore");
                });

            modelBuilder.Entity("OnPOS.Models.dbSalesDtl", b =>
                {
                    b.Property<string>("article")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("cat")
                        .HasColumnType("varchar(50)");

                    b.Property<int>("disc_amount")
                        .HasColumnType("int");

                    b.Property<int>("disc_prc")
                        .HasColumnType("int");

                    b.Property<int>("discountcode")
                        .HasColumnType("int");

                    b.Property<string>("invoice")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int>("price")
                        .HasColumnType("int");

                    b.Property<int>("qty")
                        .HasColumnType("int");

                    b.Property<int>("s33")
                        .HasColumnType("int");

                    b.Property<int>("s34")
                        .HasColumnType("int");

                    b.Property<int>("s35")
                        .HasColumnType("int");

                    b.Property<int>("s36")
                        .HasColumnType("int");

                    b.Property<int>("s37")
                        .HasColumnType("int");

                    b.Property<int>("s38")
                        .HasColumnType("int");

                    b.Property<int>("s39")
                        .HasColumnType("int");

                    b.Property<int>("s40")
                        .HasColumnType("int");

                    b.Property<int>("s41")
                        .HasColumnType("int");

                    b.Property<int>("s42")
                        .HasColumnType("int");

                    b.Property<int>("s43")
                        .HasColumnType("int");

                    b.Property<int>("s44")
                        .HasColumnType("int");

                    b.Property<int>("s45")
                        .HasColumnType("int");

                    b.Property<int>("s46")
                        .HasColumnType("int");

                    b.Property<int>("store_id")
                        .HasColumnType("int");

                    b.Property<string>("subcat")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("transdate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("update_date")
                        .HasColumnType("datetime");

                    b.Property<string>("update_user")
                        .HasColumnType("varchar(255)");

                    b.ToTable("dbSalesDtl");
                });

            modelBuilder.Entity("OnPOS.Models.dbSalesHdr", b =>
                {
                    b.Property<int>("Store_id")
                        .HasColumnType("int");

                    b.Property<string>("approval_code")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("cardnum")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("invoice")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int>("s41")
                        .HasColumnType("int");

                    b.Property<int>("staff_id")
                        .HasColumnType("int");

                    b.Property<int>("trans_amount")
                        .HasColumnType("int");

                    b.Property<int>("trans_qty")
                        .HasColumnType("int");

                    b.Property<DateTime>("transdate")
                        .HasColumnType("datetime");

                    b.Property<string>("transtype")
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("update_date")
                        .HasColumnType("datetime");

                    b.Property<string>("update_user")
                        .HasColumnType("varchar(255)");

                    b.ToTable("dbSalesHdr");
                });

            modelBuilder.Entity("OnPOS.Models.dbSalesStaff", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("COMPANY_ID")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("ENTRY_DATE")
                        .HasColumnType("datetime");

                    b.Property<string>("ENTRY_USER")
                        .HasColumnType("varchar(255)");

                    b.Property<byte[]>("FILE_PHOTO")
                        .HasColumnType("MediumBlob");

                    b.Property<string>("FILE_PHOTO_NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FLAG_AKTIF")
                        .HasColumnType("varchar(1)");

                    b.Property<string>("SALES_BLACKLIST_FLAG")
                        .HasColumnType("varchar(1)");

                    b.Property<string>("SALES_EMAIL")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("SALES_KTP")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("SALES_NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("SALES_PHONE")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("SALES_REG_DATE")
                        .HasColumnType("date");

                    b.Property<int>("STORE_ID")
                        .HasColumnType("int");

                    b.Property<DateTime>("UPDATE_DATE")
                        .HasColumnType("datetime");

                    b.Property<string>("UPDATE_USER")
                        .HasColumnType("varchar(255)");

                    b.HasKey("id")
                        .HasName("PK_SalesStaff");

                    b.ToTable("dbSalesStaff");
                });

            modelBuilder.Entity("OnPOS.Models.dbStoreList", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("COMPANY_ID")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("ENTRY_DATE")
                        .HasColumnType("datetime");

                    b.Property<string>("ENTRY_USER")
                        .HasColumnType("varchar(255)");

                    b.Property<byte[]>("FILE_PHOTO")
                        .HasColumnType("MediumBlob");

                    b.Property<string>("FILE_PHOTO_NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FLAG_AKTIF")
                        .HasColumnType("varchar(1)");

                    b.Property<string>("STORE_ADDRESS")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("STORE_BANK_BRANCH")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("STORE_BANK_COUNTRY")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("STORE_BANK_NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("STORE_BANK_NUMBER")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("STORE_BL_FLAG")
                        .HasColumnType("varchar(1)");

                    b.Property<string>("STORE_CITY")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("STORE_EMAIL")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("STORE_MANAGER_EMAIL")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("STORE_MANAGER_KTP")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("STORE_MANAGER_NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("STORE_MANAGER_PHONE")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("STORE_NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("STORE_POSTAL")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("STORE_PROVINCE")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("STORE_REG_DATE")
                        .HasColumnType("date");

                    b.Property<DateTime>("UPDATE_DATE")
                        .HasColumnType("datetime");

                    b.Property<string>("UPDATE_USER")
                        .HasColumnType("varchar(255)");

                    b.HasKey("id")
                        .HasName("PK_StoreList");

                    b.ToTable("dbStoreList");
                });

            modelBuilder.Entity("OnPOS.Models.dbStoreStockModel", b =>
                {
                    b.Property<int>("stock_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Current_stock")
                        .HasColumnType("int");

                    b.Property<int>("Past_stock")
                        .HasColumnType("int");

                    b.Property<string>("area")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("bin_id")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("bucket_id")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("cat")
                        .HasColumnType("varchar(50)");

                    b.Property<int>("dispatch_qty")
                        .HasColumnType("int");

                    b.Property<string>("itemid")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("itmname")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("lastoutdate")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("lastrcvdate")
                        .HasColumnType("datetime");

                    b.Property<string>("rack")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("racklvl")
                        .HasColumnType("varchar(50)");

                    b.Property<int>("receive_qty")
                        .HasColumnType("int");

                    b.Property<string>("row")
                        .HasColumnType("varchar(50)");

                    b.Property<int>("s33")
                        .HasColumnType("int");

                    b.Property<int>("s34")
                        .HasColumnType("int");

                    b.Property<int>("s35")
                        .HasColumnType("int");

                    b.Property<int>("s36")
                        .HasColumnType("int");

                    b.Property<int>("s37")
                        .HasColumnType("int");

                    b.Property<int>("s38")
                        .HasColumnType("int");

                    b.Property<int>("s39")
                        .HasColumnType("int");

                    b.Property<int>("s40")
                        .HasColumnType("int");

                    b.Property<int>("s41")
                        .HasColumnType("int");

                    b.Property<int>("s42")
                        .HasColumnType("int");

                    b.Property<int>("s43")
                        .HasColumnType("int");

                    b.Property<int>("s44")
                        .HasColumnType("int");

                    b.Property<int>("s45")
                        .HasColumnType("int");

                    b.Property<int>("s46")
                        .HasColumnType("int");

                    b.Property<int>("sales_qty")
                        .HasColumnType("int");

                    b.Property<int>("standard_qty")
                        .HasColumnType("int");

                    b.Property<int>("storeid")
                        .HasColumnType("int");

                    b.Property<string>("subcat")
                        .HasColumnType("varchar(50)");

                    b.HasKey("stock_id");

                    b.ToTable("dbStoreStock");
                });

            modelBuilder.Entity("OnPOS.Models.dbSubCategory", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("COMPANY_ID")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("ENTRY_DATE")
                        .HasColumnType("date");

                    b.Property<string>("ENTRY_USER")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("FLAG_AKTIF")
                        .HasColumnType("varchar(1)");

                    b.Property<string>("SubCategory")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("UPDATE_DATE")
                        .HasColumnType("date");

                    b.Property<string>("UPDATE_USER")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("description")
                        .HasColumnType("varchar(100)");

                    b.HasKey("id")
                        .HasName("PK_SubCategory");

                    b.ToTable("dbSubCategory");
                });
#pragma warning restore 612, 618
        }
    }
}
