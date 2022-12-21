using System;
using CommercialClothes.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Hosting;
using Model.Entities;

#nullable disable

namespace CommercialClothes.Models
{
    public partial class ECommerceSellingClothesContext : DbContext
    {
        public ECommerceSellingClothesContext()
        {
        }

        public ECommerceSellingClothesContext(DbContextOptions<ECommerceSellingClothesContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>()
                .HasOne(e => e.Order)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<BankType> BankTypes { get; set; }
        public virtual DbSet<Credential> Credentials { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Shop> Shops { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<HistoryTransaction> HistoryTransactions { get; set; }
    }
}
