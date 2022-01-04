using Ardalis.ApiEndpoints;
using AutoMapper;
using LocationSearch.ApplicationCore.Entities;
using LocationSearch.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LocationSearch.API.LocationEndPoints
{
    public class List : BaseAsyncEndpoint
        .WithRequest<ListLocationRequest>
        .WithResponse<ListLocationResponse>
    {
        private readonly ILogger<List> _logger;
        private readonly ILocationService _locationService;
        private readonly IMapper _mapper;

        public List(ILoggerFactory loggerFactory,
            ILocationService locationService,
            IMapper mapper)
        {
            _logger = loggerFactory.CreateLogger<List>();
            _locationService = locationService;
            _mapper = mapper;
        }

        [HttpGet("api/getlocations")]
        [SwaggerOperation(
        Summary = "List Locations",
        Description = "List Locations",
        OperationId = "getlocations.List",
        Tags = new[] { "LocationEndpoints" })
        ]
        [SwaggerResponse(200, "Ok")]
        [SwaggerResponse(400, "BadRequest")]
        [SwaggerResponse(500, "Problem")]
        public override async Task<ActionResult<ListLocationResponse>> HandleAsync([FromQuery] ListLocationRequest request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"API {nameof(List)} GetLocations called.");

            if (request.Location == null)
                return BadRequest("Location must be set");

            if (request.MaxResults < 1)
                return BadRequest("MaxResult can't be less than 1");

            if (request.MaxDistance < 0)
                return BadRequest("MaxDistance can't be less than 0");

            var response = new ListLocationResponse();

            var locations = await _locationService.GetLocationsAsync(_mapper.Map<LocationRequest>(request));

            if (locations == null)
                return Problem("Can't get locations.");

            response.Locations.AddRange(locations.Select(_mapper.Map<LocationDto>));

            return Ok(response);
        }
    }
}
