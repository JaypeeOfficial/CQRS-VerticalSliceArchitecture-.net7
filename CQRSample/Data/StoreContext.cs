using CQRSample.Domain;
using Microsoft.EntityFrameworkCore;

namespace CQRSample.Data
{
    public class StoreContext : DbContext
    {

        public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DevConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
