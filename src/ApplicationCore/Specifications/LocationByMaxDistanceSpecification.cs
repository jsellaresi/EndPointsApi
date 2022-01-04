using Ardalis.Specification;
using LocationSearch.ApplicationCore.Entities;

namespace LocationSearch.ApplicationCore.Specifications
{
    public class LocationByMaxDistanceSpecification : Specification<Location>
    {
        public LocationByMaxDistanceSpecification(MaxDistanceSquare maxDistanceSquare)
        {
            Query
                .Where(loc => (maxDistanceSquare.MinLatitude <= loc.Latitude && loc.Latitude <= maxDistanceSquare.MaxLatitude) &&
                              (maxDistanceSquare.MinLongitude <= loc.Longitude && loc.Longitude <= maxDistanceSquare.MaxLongitude));
        }
    }
}
