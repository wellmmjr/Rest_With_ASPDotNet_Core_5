using Microsoft.EntityFrameworkCore;

namespace RestWithASPNet.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext() {}
        
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) {}

        public DbSet<Person> People { get; set; }

        public DbSet<Book> Book { get; set; }
        public DbSet<User> User { get; set; }
    }
}
