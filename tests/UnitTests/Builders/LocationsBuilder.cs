using LocationSearch.ApplicationCore.Entities;
using System.Collections.Generic;

namespace UnitTests.Builders
{
    public class LocationsBuilder
    {
        private readonly List<Location> _locations;

        public LocationsBuilder()
        {
            _locations = new List<Location>();
        }

        public List<Location> GetEmptyLocations()
        {
            return _locations;
        }

        public List<Location> GetNearestLocations(Location location)
        {
            for (int i = 1; i < 5; i++)
            {
                double desviation = i * 0.00100000;

                _locations.Add(new Location($"street{i}", location.Latitude + desviation, location.Longitude - desviation));
            }

            return _locations;
        }

        public List<Location> GetFarestLocations(Location location)
        {
            for (int i = 1; i < 5; i++)
            {
                double desviation = i * 0.0100000;

                _locations.Add(new Location($"street{i}", location.Latitude + desviation, location.Longitude - desviation));
            }

            return _locations;
        }
    }
}
