using Microsoft.EntityFrameworkCore;

namespace RestWithASPNETUdemy.Models
{
    public class MySQLContext : DbContext
    {
        public MySQLContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<User> Users { get; set; }

    }
}
