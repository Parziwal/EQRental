using EQRental.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EQRental.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rental>().ToTable("Rental");
            modelBuilder.Entity<Equipment>().ToTable("Equipment");
            modelBuilder.Entity<ApplicationUser>().ToTable("User");
            modelBuilder.Entity<UserAddress>().ToTable("UserAddress");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Status>().ToTable("Status");
            modelBuilder.Entity<Payment>().ToTable("Payment");
        }
    }
}
