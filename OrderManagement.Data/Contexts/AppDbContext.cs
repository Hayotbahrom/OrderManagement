using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Data.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureProduct(modelBuilder);
            ConfigureOrder(modelBuilder);
            ConfigureOrderItem(modelBuilder);
        }

        private static void ConfigureProduct(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(p => p.Price)
                    .HasColumnType("decimal(18,2)");

                entity.Property(p => p.StockQuantity)
                    .IsRequired();

                entity.Property(p => p.CreatedAt)
                    .IsRequired();

                entity.HasIndex(p => p.Name);
            });
        }

        private static void ConfigureOrder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);

                entity.Property(o => o.CustomerName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(o => o.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(o => o.TotalAmount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(o => o.CreatedAt)
                    .IsRequired();

                entity.HasMany(o => o.Items)
                    .WithOne(i => i.Order)
                    .HasForeignKey(i => i.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(o => o.Status);
                entity.HasIndex(o => o.CreatedAt);
            });
        }

        private static void ConfigureOrderItem(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(i => i.Id);

                entity.Property(i => i.Quantity)
                    .IsRequired();

                entity.Property(i => i.Price)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(i => i.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(i => i.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
