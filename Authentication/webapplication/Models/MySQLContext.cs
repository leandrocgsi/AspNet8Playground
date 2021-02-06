using Microsoft.EntityFrameworkCore;

namespace webapplication.Models
{
    public class MySQLContext : DbContext
    {
        public MySQLContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<LoginModel> LoginModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoginModel>().HasData(new LoginModel
            {
                Id = 1,
                UserName = "leandro",
                Password = "admin123"
            });
        }
    }
}
