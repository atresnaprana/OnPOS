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
    [Migration("20211015085334_changeidxstore")]
    partial class changeidxstore
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.17")
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

            modelBuilder.Entity("BataAppHR.Models.VaksinModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DAYS_LENGTH")
                        .HasColumnType("int");

                    b.Property<string>("EDP_CODE")
                        .IsRequired()
                        .HasColumnType("varchar(5)");

                    b.Property<string>("EMAIL_SS")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("EMERGENCY_ADDRESS")
                        .HasColumnType("varchar(300)");

                    b.Property<string>("EMERGENCY_NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("EMERGENCY_PHONE")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("ENTRY_DATE")
                        .HasColumnType("date");

                    b.Property<string>("ENTRY_USER")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FILE_MEDIC")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("FLAG_AKTIF")
                        .HasColumnType("int");

                    b.Property<string>("FOTOSERTIFIKAT1")
                        .HasColumnType("varchar(300)");

                    b.Property<string>("FOTOSERTIFIKAT2")
                        .HasColumnType("varchar(300)");

                    b.Property<string>("HP_SS")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("JOIN_DATE")
                        .HasColumnType("date");

                    b.Property<string>("KTP")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LAMA_KERJA")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("MONTH_LENGTH")
                        .HasColumnType("int");

                    b.Property<string>("NAMA_SS")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("POSITION")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("RESIGN_DATE")
                        .HasColumnType("date");

                    b.Property<string>("RESIGN_TXT")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RESIGN_TYPE")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RESIGN_TYPE2")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("SEX")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("SIZE_SEPATU_UK")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("SIZE_SERAGAM")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("SS_CODE")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("STAFF_PHOTO")
                        .HasColumnType("varchar(300)");

                    b.Property<DateTime?>("UPDATE_DATE")
                        .HasColumnType("date");

                    b.Property<string>("UPDATE_USER")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("VAKSIN1")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("VAKSIN2")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("XSTORE_LOGIN")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("YEAR_LENGTH")
                        .HasColumnType("int");

                    b.HasKey("ID")
                        .HasName("PK_SSTable");

                    b.ToTable("SSTable");
                });

            modelBuilder.Entity("BataAppHR.Models.VaksinSeqModel", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.HasKey("id")
                        .HasName("PK_SSTable_Seq");

                    b.ToTable("sstable_seq");
                });

            modelBuilder.Entity("BataAppHR.Models.XstoreModel", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RD_CODE")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("area")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("district")
                        .HasColumnType("varchar(5)");

                    b.Property<string>("edp")
                        .HasColumnType("varchar(5)");

                    b.Property<string>("genesis_Flag")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("inactive_flag")
                        .HasColumnType("varchar(1)");

                    b.Property<string>("store_location")
                        .HasColumnType("varchar(75)");

                    b.HasKey("id")
                        .HasName("PK_xstore_organizations_num");

                    b.ToTable("xstore_organization");
                });

            modelBuilder.Entity("BataAppHR.Models.dbRD", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DAYS_LENGTH")
                        .HasColumnType("int");

                    b.Property<string>("EDP_CODE")
                        .HasColumnType("varchar(5)");

                    b.Property<string>("EMERGENCY_ADDRESS")
                        .HasColumnType("varchar(300)");

                    b.Property<string>("EMERGENCY_NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("EMERGENCY_PHONE")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("ENTRY_DATE")
                        .HasColumnType("date");

                    b.Property<string>("ENTRY_USER")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FILE_MEDIC")
                        .HasColumnType("varchar(300)");

                    b.Property<string>("FILE_SERTIFIKAT1")
                        .HasColumnType("varchar(300)");

                    b.Property<string>("FILE_SERTIFIKAT2")
                        .HasColumnType("varchar(300)");

                    b.Property<int>("FLAG_AKTIF")
                        .HasColumnType("int");

                    b.Property<DateTime?>("JOIN_DATE")
                        .HasColumnType("date");

                    b.Property<string>("LAMA_KERJA")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("MONTH_LENGTH")
                        .HasColumnType("int");

                    b.Property<string>("NM_RD")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("No_KTP")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RD_CODE")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("RD_EMAIL")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RD_HP")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RD_PHOTO")
                        .HasColumnType("varchar(300)");

                    b.Property<string>("RD_SEPATU_SIZEUK")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RD_SERAGAM_SIZE")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("RESIGN_DATE")
                        .HasColumnType("date");

                    b.Property<string>("RESIGN_TXT")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RESIGN_TYPE")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RESIGN_TYPE2")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("SEX")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("UPDATE_DATE")
                        .HasColumnType("date");

                    b.Property<string>("UPDATE_USER")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("VAKSIN1")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("VAKSIN2")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("YEAR_LENGTH")
                        .HasColumnType("int");

                    b.HasKey("id")
                        .HasName("PK_RD_num");

                    b.ToTable("dbRD");
                });

            modelBuilder.Entity("BataAppHR.Models.dbRDSeq", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.HasKey("id")
                        .HasName("PK_RD_Seq");

                    b.ToTable("dbRDSeq");
                });
#pragma warning restore 612, 618
        }
    }
}
