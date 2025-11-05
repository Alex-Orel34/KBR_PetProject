using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace KBR.DbStuff
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<KBRContext>
    {
        public KBRContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<KBRContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                // Fallback connection string for design time
                connectionString = "Host=localhost;Port=5432;Database=kbr;Username=postgres;Password=qwer1234";
            }

            optionsBuilder.UseNpgsql(connectionString);

            return new KBRContext(optionsBuilder.Options);
        }
    }
}
