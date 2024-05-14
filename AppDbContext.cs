using Microsoft.EntityFrameworkCore;

namespace FunctionApp1
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Products> Products { get; set; }

    }
}
