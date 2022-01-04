using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocationSearch.ApplicationCore.Entities
{
    [NotMapped]
    public class MaxDistanceSquare
    {

        public MaxDistanceSquare(Location location, int maxDistance)
        {
            var minKilometerPerLatitude = 110.5667;
            var kmPerLongitude = Math.Cos(location.Latitude) * 111.3215;
            MinLatitude = location.Latitude - maxDistance / minKilometerPerLatitude;
            MaxLatitude = location.Latitude + maxDistance / minKilometerPerLatitude;
            MinLongitude = location.Longitude - maxDistance / kmPerLongitude;
            MaxLongitude = location.Longitude + maxDistance / kmPerLongitude;
        }

        public double MinLatitude { get; private set; }

        public double MaxLatitude { get; private set; }

        public double MinLongitude { get; private set; }

        public double MaxLongitude { get; private set; }
    }
}





