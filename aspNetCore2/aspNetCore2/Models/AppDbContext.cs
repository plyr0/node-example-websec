using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace aspNetCore2.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base() { }
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options){ }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json").Build();
                optionsBuilder.UseSqlServer(config["connectionString"]);
            }
        }
        
        public DbSet<SessionModel> Sessions { get; set; }
        public DbSet<TwitModel> Twits { get; set; }
    }
}
