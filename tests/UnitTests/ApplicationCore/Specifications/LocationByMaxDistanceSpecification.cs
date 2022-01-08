using LocationSearch.ApplicationCore.Entities;
using System.Linq;
using UnitTests.Builders;
using Xunit;

namespace UnitTests.ApplicationCore.Specifications
{
    public class LocationByMaxDistanceSpecification
    {
        [Fact]
        public void NoLocationsLoaded()
        {
            int maxDistance = 10;
            int expectedCount = 0;
            var maxDistanceSquare = new MaxDistanceSquare(GetLocation(), maxDistance);

            var spec = new LocationSearch.ApplicationCore.Specifications.LocationByMaxDistanceSpecification(maxDistanceSquare);

            var result = new LocationsBuilder().GetEmptyLocations()
                                        .AsQueryable()
                                        .Where(spec.WhereExpressions.FirstOrDefault());

            Assert.Equal(expectedCount, result.Count());
        }

        [Theory]
        [InlineData(100, 0)]
        [InlineData(1000, 4)]
        [InlineData(1500, 4)]
        [InlineData(5000, 4)]
        public void MatchExpectedNumberOfNearLocations(int maxDistance, int expectedCount)
        {
            var location = GetLocation();

            var maxDistanceSquare = new MaxDistanceSquare(location, maxDistance);

            var spec = new LocationSearch.ApplicationCore.Specifications.LocationByMaxDistanceSpecification(maxDistanceSquare);

            var result = new LocationsBuilder().GetNearestLocations(location)
                                        .AsQueryable()
                                        .Where(spec.WhereExpressions.FirstOrDefault());

            Assert.Equal(expectedCount, result.Count());
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2000, 1)]
        [InlineData(3000, 2)]
        [InlineData(10000, 4)]
        public void MatchExpectedNumberOfFarLocations(int maxDistance, int expectedCount)
        {
            var location = GetLocation();

            var maxDistanceSquare = new MaxDistanceSquare(location, maxDistance);

            var spec = new LocationSearch.ApplicationCore.Specifications.LocationByMaxDistanceSpecification(maxDistanceSquare);

            var result = new LocationsBuilder().GetFarestLocations(location)
                                        .AsQueryable()
                                        .Where(spec.WhereExpressions.FirstOrDefault());

            Assert.Equal(expectedCount, result.Count());
        }

        private Location GetLocation()
        {
            return new Location("testStreet", 50.8440597, 5.7277659);
        }
    }
}
