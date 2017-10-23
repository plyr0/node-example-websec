using Microsoft.EntityFrameworkCore;

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
                optionsBuilder.UseSqlServer(Program.Configuration["connectionString"]);
            }
        }
        
        public DbSet<SessionModel> Sessions { get; set; }
    }
}
