
using LocationSearch.ApplicationCore.Entities;
using LocationSearch.ApplicationCore.Interfaces;
using LocationSearch.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnitTests.Builders;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.ApplicationCore.Services.LocationServiceTests
{
    public class GetLocationsAsync
    {
        private readonly LocationContext _dbContext;
        private readonly IRepository<Location> _repository;
        private readonly ILoggerFactory _loggerFactory = new NullLoggerFactory();
        private readonly ITestOutputHelper _output;

        public GetLocationsAsync(ITestOutputHelper output)
        {
            _output = output;

            var dbOptions = new DbContextOptionsBuilder<LocationContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            _dbContext = new LocationContext(dbOptions);
            _repository = new EfRepository<Location>(_dbContext);
        }

        [Fact]
        public async Task NoNearLocationsFound()
        {
            int expectedCount = 0;

            var service = new LocationSearch.ApplicationCore.Services.LocationService(_loggerFactory, _repository);

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

            _dbContext.Locations.AddRange(locationsBuilder.GetNearestLocations(locationRequest.Location));
            _dbContext.SaveChanges();

            var service = new LocationSearch.ApplicationCore.Services.LocationService(_loggerFactory, _repository);

            var locations = await service.GetLocationsAsync(locationRequest);

            Assert.Equal(expectedCount, locations.Count());

            if (locations.Count() > 1)
            {
                var firstLocation = locations.First();
                var lastLocation = locations.Last();

                Assert.True(firstLocation.CalculateDistance(locationRequest.Location) < lastLocation.CalculateDistance(locationRequest.Location));
            }
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

            _dbContext.Locations.AddRange(locationsBuilder.GetFarestLocations(locationRequest.Location));
            _dbContext.SaveChanges();

            var service = new LocationSearch.ApplicationCore.Services.LocationService(_loggerFactory, _repository);

            var locations = await service.GetLocationsAsync(locationRequest);

            Assert.Equal(expectedCount, locations.Count());

            if (locations.Count() > 1)
            {
                var firstLocation = locations.First();
                var lastLocation = locations.Last();

                Assert.True(firstLocation.CalculateDistance(locationRequest.Location) < lastLocation.CalculateDistance(locationRequest.Location));
            }
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
