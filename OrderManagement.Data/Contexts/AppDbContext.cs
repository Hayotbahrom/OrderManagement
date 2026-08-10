using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
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

            SeedData(modelBuilder);
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

        private static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Lenovo ThinkPad X1",
                    Price = 1250.00m,
                    StockQuantity = 13,
                    CreatedAt = new DateTime(2026, 8, 1, 9, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 2,
                    Name = "Logitech MX Master 3",
                    Price = 95.50m,
                    StockQuantity = 35,
                    CreatedAt = new DateTime(2026, 8, 1, 9, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 3,
                    Name = "Keychron K2",
                    Price = 180.00m,
                    StockQuantity = 24,
                    CreatedAt = new DateTime(2026, 8, 1, 9, 0, 0, DateTimeKind.Utc)
                });

            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    CustomerName = "Aziz Karimov",
                    Status = OrderStatus.Completed,
                    TotalAmount = 2786.50m,
                    CreatedAt = new DateTime(2026, 8, 5, 11, 30, 0, DateTimeKind.Utc)
                },
                new Order
                {
                    Id = 2,
                    CustomerName = "Dilnoza Rahimova",
                    Status = OrderStatus.Completed,
                    TotalAmount = 180.00m,
                    CreatedAt = new DateTime(2026, 8, 8, 14, 15, 0, DateTimeKind.Utc)
                },
                new Order
                {
                    Id = 3,
                    CustomerName = "Sardor Toshev",
                    Status = OrderStatus.New,
                    TotalAmount = 191.00m,
                    CreatedAt = new DateTime(2026, 8, 10, 10, 0, 0, DateTimeKind.Utc)
                });

            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 2, Price = 1250.00m },
                new OrderItem { Id = 2, OrderId = 1, ProductId = 2, Quantity = 3, Price = 95.50m },
                new OrderItem { Id = 3, OrderId = 2, ProductId = 3, Quantity = 1, Price = 180.00m },
                new OrderItem { Id = 4, OrderId = 3, ProductId = 2, Quantity = 2, Price = 95.50m });
        }
    }
}
