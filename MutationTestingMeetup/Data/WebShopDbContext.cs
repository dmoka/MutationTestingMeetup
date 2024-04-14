﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MutationTestingMeetup.Domain;

namespace MutationTestingMeetup.Data
{
    public class WebShopDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;


        public DbSet<Product> Products { get; set; }

        public DbSet<StockLevel> InventoryLevels { get; set; }



        public WebShopDbContext(DbContextOptions<WebShopDbContext> options, IDomainEventDispatcher dispatcher) : base(options)
        {
            Database.EnsureCreated();
            _dispatcher = dispatcher;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WebShopDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var entitiesWithEvents = ChangeTracker.Entries()
                .Select(e => e.Entity as BaseEntity)
                .Where(e => e != null && e.DomainEvents.Any())
                .ToList();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.DomainEvents.ToArray();
                entity.ClearDomainEvents();
                foreach (var domainEvent in events)
                {
                    await _dispatcher.Dispatch(domainEvent);
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
