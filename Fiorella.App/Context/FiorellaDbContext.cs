using Fiorella.App.Models;
using Microsoft.EntityFrameworkCore;

namespace Fiorella.App.Context
{
    public class FiorellaDbContext : DbContext
    {

        public FiorellaDbContext(DbContextOptions<FiorellaDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
