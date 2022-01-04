using LocationSearch.ApplicationCore.Entities;
using LocationSearch.ApplicationCore.Interfaces;
using LocationSearch.ApplicationCore.Specifications;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationSearch.ApplicationCore.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILogger<LocationService> _logger;
        private readonly IRepository<Location> _locationRepository;

        public LocationService(ILoggerFactory loggerFactory, IRepository<Location> locationRepository)
        {
            _logger = loggerFactory.CreateLogger<LocationService>();
            _locationRepository = locationRepository;
        }

        public async Task<IEnumerable<Location>> GetLocationsAsync(LocationRequest request)
        {
            try
            {
                _logger.LogInformation($"{nameof(LocationService)} {nameof(GetLocationsAsync)} called.");

                var maxDistanceSquare = new MaxDistanceSquare(request.Location, request.MaxDistance);

                var locationOrderedByDistanceSpecification = new LocationByMaxDistanceSpecification(maxDistanceSquare);

                var nearestLocations = await _locationRepository.ListAsync(locationOrderedByDistanceSpecification);

                var locationsOrdered = nearestLocations
                                    .OrderBy(location => location.CalculateDistance(request.Location))
                                    .Take(request.MaxResults)
                                    .ToList();

                return locationsOrdered;

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"{nameof(LocationService)} {nameof(GetLocationsAsync)} error.");
                return null;
            }
        }
    }
}
