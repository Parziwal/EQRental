using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Main
{
    class EQRentalContext : DbContext
    {
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rental>().ToTable("Rental");
            modelBuilder.Entity<Equipment>().ToTable("Equipment");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<UserAddress>().ToTable("UserAddress");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Status>().ToTable("Status");
            modelBuilder.Entity<Payment>().ToTable("Payment");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=EQRentalMaster.db");
        }
    }
}
