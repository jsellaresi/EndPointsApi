using System;
using System.ComponentModel.DataAnnotations;

namespace LocationSearch.ApplicationCore.Entities
{
    public class Location
    {
        public Location()
        {

        }

        public Location(string address, double latitude, double longitude)
        {
            Address = address;
            Latitude = latitude;
            Longitude = longitude;
        }

        [Key]
        public long Id { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }


        /// <summary>
        /// Calculates the distance between this location and another one, in meters.
        /// </summary>
        public double CalculateDistance(Location location)
        {
            if (location == null)
                throw new Exception("Location must be filled");

            if (location.Latitude == Latitude && location.Longitude == Longitude)
                return 0;

            var currentLocationLatitudeRadians = degreesToRadians(Latitude);
            var locationRequestedLatitudeRadians = degreesToRadians(location.Latitude);

            var theta = Longitude - location.Longitude;
            var thetaRadians = degreesToRadians(theta);

            var distance = Math.Sin(currentLocationLatitudeRadians) * Math.Sin(locationRequestedLatitudeRadians) + Math.Cos(currentLocationLatitudeRadians) * Math.Cos(locationRequestedLatitudeRadians) * Math.Cos(thetaRadians);
            distance = Math.Acos(distance);
            distance = degreesToRadians(distance);

            distance = distance * 60 * 1.1515;

            return distance * 1609.344;
        }

        public override string ToString()
        {
            return $"{Address} (latitude {Latitude}, longitude {Longitude})";
        }

        private double degreesToRadians(double degrees)
        {
            return (degrees * Math.PI / 180.0);
        }
    }
}
