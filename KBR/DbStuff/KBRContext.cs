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
                    .HasDefaultValue(Role.User)
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
                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasConversion(
                        v => v.Kind == DateTimeKind.Unspecified 
                            ? DateTime.SpecifyKind(v, DateTimeKind.Utc)
                            : v.ToUniversalTime(),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
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

        /// <summary>
        /// Инициализация категорий по умолчанию для нового пользователя
        /// (Пока заглушка - не знаю как сделать через скрипты на базу)
        /// </summary>
        public async Task SeedDefaultCategoriesAsync(Guid userId)
        {
            var defaultCategories = new List<(string Name, string Color, string Icon)>
            {
                ("Автоуслуги", "#FF6B6B", "bi-car-front"),
                ("АЗС", "#4ECDC4", "bi-fuel-pump"),
                ("Алкоголь", "#95E1D3", "bi-cup-straw"),
                ("Аптеки", "#F38181", "bi-heart-pulse"),
                ("Детские товары", "#AA96DA", "bi-balloon"),
                ("Дом и ремонт", "#FCBAD3", "bi-hammer"),
                ("Заработная плата", "#95E1D3", "bi-wallet2"),
                ("Здоровье", "#F38181", "bi-heart"),
                ("Кафе и рестораны", "#FFD93D", "bi-cup-hot"),
                ("Книги и канцтовары", "#6BCB77", "bi-book"),
                ("Коммунальные услуги", "#4D96FF", "bi-house"),
                ("Красота", "#FF6B9D", "bi-palette"),
                ("Маркетплейсы", "#C44569", "bi-cart"),
                ("Одежда и обувь", "#F8B500", "bi-bag"),
                ("Супермаркеты", "#6BCB77", "bi-shop"),
                ("Такси", "#4ECDC4", "bi-taxi-front"),
                ("Транспорт", "#4D96FF", "bi-bus-front"),
                ("Цветы", "#FF6B9D", "bi-flower1"),
                ("Электроника", "#95A5A6", "bi-device-ssd")
            };

            var existingCategories = await Categories
                .Where(c => c.UserId == userId)
                .Select(c => c.CategoryName)
                .ToListAsync();

            var categoriesToAdd = defaultCategories
                .Where(c => !existingCategories.Contains(c.Name))
                .Select(c => new Category
                {
                    Id = Guid.NewGuid(),
                    CategoryName = c.Name,
                    Color = c.Color,
                    IconUrl = c.Icon,
                    UserId = userId
                })
                .ToList();

            if (categoriesToAdd.Any())
            {
                await Categories.AddRangeAsync(categoriesToAdd);
                await SaveChangesAsync();
            }
        }
    }
}
