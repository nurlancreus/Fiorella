using Fiorella.App.Models;
using Fiorella.App.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Fiorella.App.Context
{
    public class FiorellaDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public FiorellaDbContext(DbContextOptions<FiorellaDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Weight)
                .HasPrecision(10, 2);

            // Configure the Product and ProductImage one-to-many relationship
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Images)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId)
                .IsRequired(false);

            // Configure the Product and Category many-to-many relationship using ProductCategory
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Categories)
                .WithMany(c => c.Products)
                .UsingEntity<ProductCategory>(
                    j => j
                        .HasOne(pc => pc.Category)
                        .WithMany(c => c.CategoryProducts)
                        .HasForeignKey(pc => pc.CategoryId),
                    j => j
                        .HasOne(pc => pc.Product)
                        .WithMany(p => p.ProductCategories)
                        .HasForeignKey(pc => pc.ProductId),
                    j =>
                    {
                        j.HasKey(pc => new { pc.ProductId, pc.CategoryId });
                    });

            // Configure the Product and Tag many-to-many relationship using ProductTag
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Tags)
                .WithMany(t => t.Products)
                .UsingEntity<ProductTag>(
                    j => j
                        .HasOne(pt => pt.Tag)
                        .WithMany(t => t.TagProducts)
                        .HasForeignKey(pt => pt.TagId),
                    j => j
                        .HasOne(pt => pt.Product)
                        .WithMany(p => p.ProductTags)
                        .HasForeignKey(pt => pt.ProductId),
                    j =>
                    {
                        j.HasKey(pt => new { pt.ProductId, pt.TagId });
                    });
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
