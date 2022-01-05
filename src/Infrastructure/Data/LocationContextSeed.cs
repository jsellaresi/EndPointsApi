using LocationSearch.Infrastructure.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LocationSearch.Infrastructure.Data
{
    public class LocationContextSeed
    {
        public static async Task SeedAsync(LocationContext locationContext, ILoggerFactory loggerFactory, int retry = 0)
        {
            var retryForAvailability = retry;
            try
            {
                if (locationContext.Database.IsSqlServer())
                {
                    locationContext.Database.Migrate();
                    locationContext.Database.EnsureCreated();

                    await SeedLocationsData.SeedInDataTable(locationContext);
                }
                else
                {
                    await SeedLocationsData.SeedInMemory(locationContext);
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability >= 10) throw;

                retryForAvailability++;
                var log = loggerFactory.CreateLogger<LocationContextSeed>();
                log.LogError(ex.Message);
                await SeedAsync(locationContext, loggerFactory, retryForAvailability);
                throw;
            }
        }
    }
}
