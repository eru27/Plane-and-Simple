﻿// <auto-generated />
using System;
using AirplaneTickets.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AirplaneTickets.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240612200836_test1")]
    partial class test1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AirplaneTickets.Models.Entities.Flight", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AvailableSeats")
                        .HasColumnType("int");

                    b.Property<int>("Connections")
                        .HasColumnType("int");

                    b.Property<int>("Departure")
                        .HasColumnType("int");

                    b.Property<DateOnly>("DepartureDate")
                        .HasColumnType("date");

                    b.Property<int>("Destination")
                        .HasColumnType("int");

                    b.Property<bool>("IsCanceled")
                        .HasColumnType("bit");

                    b.Property<int>("TotalSeats")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("AirplaneTickets.Models.Entities.UserAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("UserAccounts");
                });
#pragma warning restore 612, 618
        }
    }
}
