using LocationSearch.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LocationSearch.Infrastructure.Data
{
    public class LocationContext : DbContext
    {
        public LocationContext(DbContextOptions<LocationContext> options) : base(options)
        {
        }

        public DbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
