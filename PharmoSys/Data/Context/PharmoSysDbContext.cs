using Microsoft.EntityFrameworkCore;
using PharmoSys.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PharmoSys.Data.Context
{
    internal class PharmoSysDbContext:DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<SupplierEntity> Suppliers { get; set; }
        public DbSet<SaleEntity> Sales { get; set; }
        public DbSet<SaleItemEntity> SaleItems { get; set; }
        public DbSet<StockHistoryEntity> StockHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=PharmoSys;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Roles → Users (1 to many)
            modelBuilder.Entity<RoleEntity>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId);

            // Suppliers → Products (1 to many)
            modelBuilder.Entity<SupplierEntity>()
                .HasMany(s => s.Products)
                .WithOne(p => p.Supplier)
                .HasForeignKey(p => p.SupplierId);

            // Users → Sales (1 to many)
            modelBuilder.Entity<UserEntity>()
                .HasMany(u => u.Sales)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId);

            // Sales → SaleItems (1 to many)
            modelBuilder.Entity<SaleEntity>()
                .HasMany(s => s.SaleItems)
                .WithOne(si => si.Sale)
                .HasForeignKey(si => si.SaleId);

            // Products → SaleItems (1 to many)
            modelBuilder.Entity<ProductEntity>()
                .HasMany(p => p.SaleItems)
                .WithOne(si => si.Product)
                .HasForeignKey(si => si.ProductId);

            // Products → StockHistory (1 to many)
            modelBuilder.Entity<ProductEntity>()
                .HasMany(p => p.StockHistories)
                .WithOne(sh => sh.Product)
                .HasForeignKey(sh => sh.ProductId);
        }
    }
}
