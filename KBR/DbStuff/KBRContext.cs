using KBR.DbStuff.Models;
using KBR.Enum;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace KBR.DbStuff
{
    public class KBRContext : DbContext
    {
        public KBRContext(DbContextOptions<KBRContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureUserRelationships(modelBuilder);
            ConfigureCategoryRelationships(modelBuilder);
            ConfigurePaymentRelationships(modelBuilder);
            ConfigureCurrencyRelationships(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ConfigureUserRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Login).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(100);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasDefaultValue(Role.user)
                    .HasConversion<int>();

                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Login).IsUnique();
            });
        }

        private void ConfigureCategoryRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CategoryName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.IconUrl).HasMaxLength(500);
                entity.Property(e => e.Color).HasMaxLength(7);
                entity.HasOne(e => e.User)
                    .WithMany(u => u.CreatedCategories)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.Payments)
                    .WithOne(p => p.Category)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigurePaymentRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PaymentSum).IsRequired().HasPrecision(18, 2);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.PaymentType)
                    .IsRequired()
                    .HasDefaultValue(PaymentType.Expense)
                    .HasConversion<int>();
                entity.HasOne(e => e.User)
                    .WithMany(u => u.CreatedPayments)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Payments)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Currency)
                    .WithMany(c => c.Payments)
                    .HasForeignKey(e => e.CurrencyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureCurrencyRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CurrencyCode).IsRequired().HasMaxLength(3);
                entity.Property(e => e.CurrencyName).HasMaxLength(50);

                entity.HasIndex(e => e.CurrencyCode).IsUnique();
            });
        }
    }
}
