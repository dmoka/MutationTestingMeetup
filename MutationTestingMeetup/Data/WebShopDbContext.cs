using Microsoft.EntityFrameworkCore;
using MutationTestingMeetup.Domain;

namespace MutationTestingMeetup.Data
{
    public class WebShopDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }


        public WebShopDbContext(DbContextOptions<WebShopDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WebShopDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
