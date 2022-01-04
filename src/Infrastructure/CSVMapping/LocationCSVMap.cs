using CsvHelper.Configuration;
using LocationSearch.ApplicationCore.Entities;

namespace LocationSearch.Infrastructure.CSVMapping
{
    public class LocationCSVMap : ClassMap<Location>
    {
        public LocationCSVMap()
        {
            Map(m => m.Address);
            Map(m => m.Latitude);
            Map(m => m.Longitude);
        }
    }
}
