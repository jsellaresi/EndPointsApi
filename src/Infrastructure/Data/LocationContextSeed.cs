using CsvHelper;
using LocationSearch.ApplicationCore.Entities;
using LocationSearch.Infrastructure.CSVMapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
                if (!await locationContext.Locations.AnyAsync())
                {
                    await locationContext.Locations.AddRangeAsync(GetPreconfiguredLocations());

                    await locationContext.SaveChangesAsync();
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

        private static IEnumerable<Location> GetPreconfiguredLocations()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "LocationSearch.Infrastructure.Data.SeedData.locations.csv";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                    csvReader.Context.RegisterClassMap<LocationCSVMap>();

                    return csvReader.GetRecords<Location>().ToArray();
                }
            }
        }
    }
}
