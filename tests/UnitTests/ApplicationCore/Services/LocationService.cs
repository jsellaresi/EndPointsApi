using LocationSearch.ApplicationCore.Entities;
using LocationSearch.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using UnitTests.Builders;
using Xunit;

namespace UnitTests.ApplicationCore.Services
{
    public class LocationService
    {
        private readonly Mock<IRepository<Location>> _mockRepository = new();
        private readonly ILoggerFactory _loggerFactory = new NullLoggerFactory();

        [Fact]
        public async Task NoNearLocationsFound()
        {
            int expectedCount = 0;
            var locationsBuilder = new LocationsBuilder();

            _mockRepository.Setup(rep => rep.ListAsync(It.IsAny<LocationSearch.ApplicationCore.Specifications.LocationByMaxDistanceSpecification>(), default)).ReturnsAsync(locationsBuilder.GetEmptyLocations());

            var service = new LocationSearch.ApplicationCore.Services.LocationService(_loggerFactory, _mockRepository.Object);

            var locations = await service.GetLocationsAsync(GetLocationRequest(10, 100));

            Assert.Equal(expectedCount, locations.Count());
        }

        [Theory]
        [InlineData(1, 4)]
        [InlineData(2, 4)]
        [InlineData(5, 4)]
        public async Task MatchNearLocations(int maxDistance, int expectedCount)
        {
            var locationsBuilder = new LocationsBuilder();

            var locationRequest = GetLocationRequest(maxDistance, 10);

            _mockRepository.Setup(rep => rep.ListAsync(It.IsAny<LocationSearch.ApplicationCore.Specifications.LocationByMaxDistanceSpecification>(), default)).ReturnsAsync(locationsBuilder.GetNearestLocations(locationRequest.Location));

            var service = new LocationSearch.ApplicationCore.Services.LocationService(_loggerFactory, _mockRepository.Object);

            var locations = await service.GetLocationsAsync(locationRequest);

            Assert.Equal(expectedCount, locations.Count());
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        [InlineData(10, 4)]
        public async Task MatchFarLocations(int maxDistance, int expectedCount)
        {
            var locationsBuilder = new LocationsBuilder();

            var locationRequest = GetLocationRequest(maxDistance, 10);

            _mockRepository.Setup(rep => rep.ListAsync(It.IsAny<LocationSearch.ApplicationCore.Specifications.LocationByMaxDistanceSpecification>(), default)).ReturnsAsync(locationsBuilder.GetFarestLocations(locationRequest.Location).Take(expectedCount).ToList());

            var service = new LocationSearch.ApplicationCore.Services.LocationService(_loggerFactory, _mockRepository.Object);

            var locations = await service.GetLocationsAsync(locationRequest);

            Assert.Equal(expectedCount, locations.Count());
        }


        private LocationRequest GetLocationRequest(int maxDistance, int maxResults)
        {
            return new LocationRequest
            {
                Location = new Location("testStreet", 50.8440597, 5.7277659),
                MaxDistance = maxDistance,
                MaxResults = maxResults,
            };
        }
    }
}
