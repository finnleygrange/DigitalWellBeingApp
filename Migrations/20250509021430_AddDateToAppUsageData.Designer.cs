﻿// <auto-generated />
using System;
using DigitalWellBeingApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DigitalWellBeingApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250509021430_AddDateToAppUsageData")]
    partial class AddDateToAppUsageData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.4");

            modelBuilder.Entity("DigitalWellBeingApp.Models.AppUsageData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AppName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("SessionCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TimeSpent")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("AppUsageData");
                });

            modelBuilder.Entity("DigitalWellBeingApp.Models.SleepEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<double>("HoursSlept")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("SleepEntries");
                });
#pragma warning restore 612, 618
        }
    }
}
