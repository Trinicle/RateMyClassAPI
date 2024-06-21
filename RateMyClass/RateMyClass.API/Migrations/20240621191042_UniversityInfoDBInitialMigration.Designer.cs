﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RateMyClass.API.DbContexts;

#nullable disable

namespace RateMyClass.API.Migrations
{
    [DbContext(typeof(UniversityInfoContext))]
    [Migration("20240621191042_UniversityInfoDBInitialMigration")]
    partial class UniversityInfoDBInitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("RateMyClass.API.Entities.Class", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UniversityId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UniversityId");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("RateMyClass.API.Entities.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClassId")
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<int>("Difficulty")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quality")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("TakeAgain")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("RateMyClass.API.Entities.University", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Website")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Zip")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Universities");
                });

            modelBuilder.Entity("RateMyClass.API.Entities.Class", b =>
                {
                    b.HasOne("RateMyClass.API.Entities.University", "University")
                        .WithMany("OfferedClasses")
                        .HasForeignKey("UniversityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("University");
                });

            modelBuilder.Entity("RateMyClass.API.Entities.Rating", b =>
                {
                    b.HasOne("RateMyClass.API.Entities.Class", "Class")
                        .WithMany("Ratings")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");
                });

            modelBuilder.Entity("RateMyClass.API.Entities.Class", b =>
                {
                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("RateMyClass.API.Entities.University", b =>
                {
                    b.Navigation("OfferedClasses");
                });
#pragma warning restore 612, 618
        }
    }
}
