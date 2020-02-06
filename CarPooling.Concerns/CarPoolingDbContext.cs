using CarPooling.Concerns;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarPooling.Concerns
{
    public class CarPoolingDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Data Source=MOUNIKAD\\MSSQLSERVER1;Initial Catalog=CarPooling;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<User> User { get; set; }

        public DbSet<Ride> Ride { get; set; }

        public DbSet<Location> Location { get; set; }

        public DbSet<Vehicle> Vehicle { get; set; }

        public DbSet<Booking> Booking { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
