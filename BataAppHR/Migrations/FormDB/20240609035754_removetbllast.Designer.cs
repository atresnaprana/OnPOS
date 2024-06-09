﻿// <auto-generated />
using System;
using BataAppHR.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BataAppHR.Migrations.FormDB
{
    [DbContext(typeof(FormDBContext))]
    [Migration("20240609035754_removetbllast")]
    partial class removetbllast
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("OnPOS.Models.dbItemMaster", b =>
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

                    b.Property<DateTime>("UPDATE_DATE")
                        .HasColumnType("date");

                    b.Property<string>("UPDATE_USER")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("brand")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("category")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("color")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("itemdescription")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("itemid")
                        .HasColumnType("varchar(50)");

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