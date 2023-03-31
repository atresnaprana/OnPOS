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
    [Migration("20210914130008_checkIfBeingLeft")]
    partial class checkIfBeingLeft
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

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
                    b.Property<string>("edp")
                        .HasColumnType("varchar(5)");

                    b.Property<string>("area")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("district")
                        .HasColumnType("varchar(5)");

                    b.Property<string>("genesis_Flag")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("inactive_flag")
                        .HasColumnType("varchar(1)");

                    b.Property<string>("store_location")
                        .HasColumnType("varchar(75)");

                    b.HasKey("edp")
                        .HasName("PK_xstore_organizations");

                    b.ToTable("xstore_organization");
                });

            modelBuilder.Entity("BataAppHR.Models.dbNilaiSS", b =>
                {
                    b.Property<int>("ScoreId")
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Entry_Date")
                        .HasColumnType("date");

                    b.Property<string>("Entry_User")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("FILE_SERTIFIKAT")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FLAG_AKTIF")
                        .HasColumnType("varchar(1)");

                    b.Property<int>("NILAI")
                        .HasColumnType("int");

                    b.Property<string>("NoSertifikat")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("SERTIFIKAT")
                        .IsRequired()
                        .HasColumnType("varchar(1)");

                    b.Property<string>("SS_CODE")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("TRN_ID")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("Update_Date")
                        .HasColumnType("date");

                    b.Property<string>("Update_User")
                        .HasColumnType("varchar(100)");

                    b.HasKey("ScoreId")
                        .HasName("PK_ScoreTbl");

                    b.ToTable("scoreDB");
                });

            modelBuilder.Entity("BataAppHR.Models.dbProgram", b =>
                {
                    b.Property<string>("ProgramId")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("Entry_Date")
                        .HasColumnType("date");

                    b.Property<string>("Entry_User")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("ProgramName")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("Update_Date")
                        .HasColumnType("date");

                    b.Property<string>("Update_User")
                        .HasColumnType("varchar(100)");

                    b.HasKey("ProgramId")
                        .HasName("PK_ProgramTbl");

                    b.ToTable("dbProgram");
                });

            modelBuilder.Entity("BataAppHR.Models.dbProgramSeq", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.HasKey("id")
                        .HasName("PK_Program_Seq");

                    b.ToTable("dbProgram_seq");
                });

            modelBuilder.Entity("BataAppHR.Models.dbRekapTraining", b =>
                {
                    b.Property<string>("TRN_ID")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("date");

                    b.Property<int>("Days")
                        .HasColumnType("int");

                    b.Property<string>("EDP")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("Entry_Date")
                        .HasColumnType("date");

                    b.Property<string>("Entry_User")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("FLAG_AKTIF")
                        .HasColumnType("varchar(1)");

                    b.Property<decimal?>("Hours")
                        .HasColumnType("decimal(6,2)");

                    b.Property<int>("NoParticipant")
                        .HasColumnType("int");

                    b.Property<string>("Participant")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Periode")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Program")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProgramTxt")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("TotalHours")
                        .HasColumnType("int");

                    b.Property<string>("Trainer")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Type")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("Update_Date")
                        .HasColumnType("date");

                    b.Property<string>("Update_User")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Week")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("idTrainer")
                        .HasColumnType("varchar(255)");

                    b.HasKey("TRN_ID")
                        .HasName("PK_RekapTrainingTbl");

                    b.ToTable("dbRekapTraining");
                });

            modelBuilder.Entity("BataAppHR.Models.dbRekapTrainingSeq", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.HasKey("id")
                        .HasName("PK_RekapTraining_Seq");

                    b.ToTable("dbRekapTraining_seq");
                });

            modelBuilder.Entity("BataAppHR.Models.dbTrainer", b =>
                {
                    b.Property<string>("idTrainer")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("Entry_Date")
                        .HasColumnType("date");

                    b.Property<string>("Entry_User")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("FLAG_AKTIF")
                        .HasColumnType("varchar(1)");

                    b.Property<string>("HP")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("NmShort")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("NmTrainer")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("Update_Date")
                        .HasColumnType("date");

                    b.Property<string>("Update_User")
                        .HasColumnType("varchar(100)");

                    b.HasKey("idTrainer")
                        .HasName("PK_TrainerTbl");

                    b.ToTable("dbTrainer");
                });

            modelBuilder.Entity("BataAppHR.Models.dbTrainerSeq", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.HasKey("id")
                        .HasName("PK_Trainer_Seq");

                    b.ToTable("dbTrainer_seq");
                });
#pragma warning restore 612, 618
        }
    }
}
