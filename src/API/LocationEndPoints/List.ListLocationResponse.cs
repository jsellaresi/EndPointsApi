using System.Collections.Generic;

namespace LocationSearch.API.LocationEndPoints
{
    public class ListLocationResponse
    {
        public List<LocationDto> Locations { get; set; } = new List<LocationDto>();
    }
}
