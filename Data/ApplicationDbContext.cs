using Microsoft.EntityFrameworkCore;
using TestAppAICodeReview.Models;

namespace TestAppAICodeReview.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);  // Example: 18 total digits, 2 decimal places
        }

    }
}
