using Microsoft.EntityFrameworkCore;

namespace RestWithASPNETUdemy.Model.Context
{
    public class PostgreeSQLContext : DbContext
    {
        public PostgreeSQLContext()
        {
        
        }
        public PostgreeSQLContext(DbContextOptions<PostgreeSQLContext> options) : base(options) {}

        public DbSet<Person> Persons { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
