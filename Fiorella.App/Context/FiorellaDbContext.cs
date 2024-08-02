using Fiorella.App.Models;
using Fiorella.App.Models.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;

namespace Fiorella.App.Context
{
    public class FiorellaDbContext(DbContextOptions<FiorellaDbContext> options) : IdentityDbContext<IdentityUser>(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProductTag> ProductTag { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Discount> Discounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply global query filter for soft delete
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(GetIsDeletedFilter(entityType.ClrType));
                }
            }

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

            base.OnModelCreating(modelBuilder);
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

        private static LambdaExpression GetIsDeletedFilter(Type entityType)
        {
            var param = Expression.Parameter(entityType, "e");
            var prop = Expression.Property(param, "IsDeleted");
            var condition = Expression.Equal(prop, Expression.Constant(false));

            return Expression.Lambda(condition, param);
        }
    }
}
