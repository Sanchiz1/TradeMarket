﻿using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Options;

namespace Data.Data
{
    public class TradeMarketDbContext : DbContext
    {
        public TradeMarketDbContext(DbContextOptions<TradeMarketDbContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons {  get; set; }
        public DbSet<Customer> Customers {  get; set; }
        public DbSet<Receipt> Receipts {  get; set; }
        public DbSet<ReceiptDetail> ReceiptsDetails {  get; set; }
        public DbSet<Product> Products {  get; set; }
        public DbSet<ProductCategory> ProductCategories {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReceiptDetail>()
                .HasKey(rd => new { rd.ReceiptId, rd.ProductId });
        }
    }
}
