namespace LocationSearch.API.LocationEndPoints
{
    public class LocationRequestDto
    {
        public LocationRequestDto()
        {
        }

        public LocationRequestDto(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}