using Microsoft.EntityFrameworkCore;

namespace webapplication.Models
{
    public class MySQLContext : DbContext
    {
        public MySQLContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<User> Users { get; set; }

    }
}
