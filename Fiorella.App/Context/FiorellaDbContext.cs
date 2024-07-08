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
    }
}
