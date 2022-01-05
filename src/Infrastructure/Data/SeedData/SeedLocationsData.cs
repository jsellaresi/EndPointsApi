using CsvHelper;
using LocationSearch.ApplicationCore.Entities;
using LocationSearch.Infrastructure.CSVMapping;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LocationSearch.Infrastructure.Data.SeedData
{
    public static class SeedLocationsData
    {
        public static async Task SeedInMemory(LocationContext locationContext)
        {
            await locationContext.Locations.AddRangeAsync(GetPreconfiguredLocations());
            await locationContext.SaveChangesAsync();
        }

        public static async Task SeedInDataTable(LocationContext locationContext)
        {
            if (!await locationContext.Locations.AnyAsync())
            {
                var locations = GetPreconfiguredLocations();

                if (locations.Count() > 0)
                {
                    const int initialCount = 1000;

                    int count = initialCount;

                    foreach (var item in locations)
                    {
                        await locationContext.Locations.AddAsync(item);
                        --count;
                        if (count <= 0)
                        {
                            count = initialCount;
                            await locationContext.SaveChangesAsync();
                        }
                    }

                    await locationContext.SaveChangesAsync(); // To make sure we save everything if the last part was smaller than initialCount
                }
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
