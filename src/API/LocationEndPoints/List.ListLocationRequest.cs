namespace LocationSearch.API.LocationEndPoints
{
    public class ListLocationRequest
    {
        public LocationRequestDto Location { get; set; }

        public int MaxDistance { get; set; } = 0;

        public int MaxResults { get; set; } = 20;
    }


}
