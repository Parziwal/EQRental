﻿// <auto-generated />
using System;
using ConsoleApp1;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ConsoleApp1.Migrations
{
    [DbContext(typeof(EQRentalContext))]
    partial class EQRentalContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8");

            modelBuilder.Entity("ConsoleApp1.Category", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("ConsoleApp1.Equipment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Available")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CategoryID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Details")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int?>("OwnerID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PricePerDay")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("OwnerID");

                    b.ToTable("Equipment");
                });

            modelBuilder.Entity("ConsoleApp1.Payment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("ConsoleApp1.Rental", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AddressID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("EquipmentID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PaymentID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("StatusID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("AddressID");

                    b.HasIndex("EquipmentID");

                    b.HasIndex("PaymentID");

                    b.HasIndex("StatusID");

                    b.ToTable("Rental");
                });

            modelBuilder.Entity("ConsoleApp1.Status", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("ConsoleApp1.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BankAccount")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("ConsoleApp1.UserAddress", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("City")
                        .HasColumnType("TEXT");

                    b.Property<int>("PostalCode")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Street")
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("UserAddress");
                });

            modelBuilder.Entity("ConsoleApp1.Equipment", b =>
                {
                    b.HasOne("ConsoleApp1.Category", "Category")
                        .WithMany("Equipments")
                        .HasForeignKey("CategoryID");

                    b.HasOne("ConsoleApp1.User", "Owner")
                        .WithMany("Equipments")
                        .HasForeignKey("OwnerID");
                });

            modelBuilder.Entity("ConsoleApp1.Rental", b =>
                {
                    b.HasOne("ConsoleApp1.UserAddress", "Address")
                        .WithMany("Rentals")
                        .HasForeignKey("AddressID");

                    b.HasOne("ConsoleApp1.Equipment", "Equipment")
                        .WithMany("Rentals")
                        .HasForeignKey("EquipmentID");

                    b.HasOne("ConsoleApp1.Payment", "Payment")
                        .WithMany("Rentals")
                        .HasForeignKey("PaymentID");

                    b.HasOne("ConsoleApp1.Status", "Status")
                        .WithMany("Rentals")
                        .HasForeignKey("StatusID");
                });

            modelBuilder.Entity("ConsoleApp1.UserAddress", b =>
                {
                    b.HasOne("ConsoleApp1.User", "User")
                        .WithMany("Addresses")
                        .HasForeignKey("UserID");
                });
#pragma warning restore 612, 618
        }
    }
}
