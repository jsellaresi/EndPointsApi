using LocationSearch.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocationSearch.Infrastructure.Data.Config
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasIndex(loc => new { loc.Latitude, loc.Longitude });
        }
    }
}
