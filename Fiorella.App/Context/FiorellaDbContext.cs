using Fiorella.App.Models;
using Fiorella.App.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Fiorella.App.Context
{
    public class FiorellaDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public FiorellaDbContext(DbContextOptions<FiorellaDbContext> options) : base(options)
        {
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var changedEntries = ChangeTracker.Entries<BaseEntity>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in changedEntries)
            {
                if (entry.State == EntityState.Added) entry.Entity.CreatedAt = DateTime.Now;
                if (entry.State == EntityState.Modified) entry.Entity.UpdatedAt = DateTime.Now;
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
