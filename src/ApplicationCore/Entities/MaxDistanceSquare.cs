using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocationSearch.ApplicationCore.Entities
{
    [NotMapped]
    public class MaxDistanceSquare
    {
        /// <summary>
        /// This entity calculates a max and min coordinates with given location and maxDistance in meters requestes.
        /// </summary>
        /// <param name="location">The given location</param>
        /// <param name="maxDistance">Max distance requested in meters</param>
        public MaxDistanceSquare(Location location, int maxDistance)
        {
            var minMetersPerLatitude = 110.5667 * 1000;
            var metersPerLongitude = Math.Cos(location.Latitude) * 111.3215 * 1000;

            MinLatitude = location.Latitude - maxDistance / minMetersPerLatitude;
            MaxLatitude = location.Latitude + maxDistance / minMetersPerLatitude;
            MinLongitude = location.Longitude - maxDistance / metersPerLongitude;
            MaxLongitude = location.Longitude + maxDistance / metersPerLongitude;
        }

        public double MinLatitude { get; private set; }

        public double MaxLatitude { get; private set; }

        public double MinLongitude { get; private set; }

        public double MaxLongitude { get; private set; }
    }
}





