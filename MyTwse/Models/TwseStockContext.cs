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
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS01;Database=TwseStock;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InsertDateLog>(entity =>
            {
                entity.HasKey(e => new { e.Type, e.Date });

                entity.Property(e => e.Date).HasColumnType("date");
            });

            modelBuilder.Entity<StockInfo>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.FinancialReport)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PB).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PE).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.YieldRate)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
