using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocationSearch.ApplicationCore.Entities
{
    [NotMapped]
    public class MaxDistanceSquare
    {

        public MaxDistanceSquare(Location location, int maxDistance)
        {
            var minKilometersPerLatitude = 110.5667 * 1000;
            var kmPerLongitude = Math.Cos(location.Latitude) * 111.3215 * 1000;

            //var minKilometersPerLatitude = 1.375 * 1000;
            //var kmPerLongitude = Math.Cos(location.Latitude) * 1.391 * 1000;

            MinLatitude = location.Latitude - maxDistance / minKilometersPerLatitude;
            MaxLatitude = location.Latitude + maxDistance / minKilometersPerLatitude;
            MinLongitude = location.Longitude - maxDistance / kmPerLongitude;
            MaxLongitude = location.Longitude + maxDistance / kmPerLongitude;
        }

        public double MinLatitude { get; private set; }

        public double MaxLatitude { get; private set; }

        public double MinLongitude { get; private set; }

        public double MaxLongitude { get; private set; }
    }
}





