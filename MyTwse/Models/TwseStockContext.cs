using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyTwse.Models
{
    public partial class TwseStockContext : DbContext
    {
        public TwseStockContext()
        {
        }

        public TwseStockContext(DbContextOptions<TwseStockContext> options)
            : base(options)
        {
        }

        public virtual DbSet<InsertDateLog> InsertDateLog { get; set; }
        public virtual DbSet<StockInfo> StockInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InsertDateLog>(entity =>
            {
                entity.HasKey(e => new { e.Type, e.Date });
            });

            modelBuilder.Entity<StockInfo>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FinancialReport)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
