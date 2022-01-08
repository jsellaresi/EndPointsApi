using LocationSearch.API.LocationEndPoints;
using LocationSearch.ApplicationCore.Extensions;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.API.LocationEndPoints
{
    public class List : IClassFixture<CustomWebApplicationFactory>
    {
        public List(CustomWebApplicationFactory factory)
        {
            Client = factory.CreateClient();
        }

        public HttpClient Client { get; }

        [Fact]
        public async Task BadRequestLocationEmpty()
        {
            var listLocationRequest = new ListLocationRequest
            {
                Location = null,
                MaxDistance = 100,
                MaxResults = 10,
            };

            var response = await Client.GetAsync(getCallUrl(listLocationRequest));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task BadRequestMaxDistance()
        {
            var listLocationRequest = GetLocationRequest(-1, 1);

            var response = await Client.GetAsync(getCallUrl(listLocationRequest));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task BadRequestMaxResults()
        {
            var listLocationRequest = GetLocationRequest(1, 0);

            var response = await Client.GetAsync(getCallUrl(listLocationRequest));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ReturnsSingleLocation()
        {
            int expectedCount = 1;
            var listLocationRequest = new ListLocationRequest
            {
                Location = new LocationRequestDto(0, 0),
                MaxDistance = 100000,
                MaxResults = 10,
            };

            var response = await Client.GetAsync(getCallUrl(listLocationRequest));
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = stringResponse.FromJson<ListLocationResponse>();

            Assert.Equal(expectedCount, model.Locations.Count());
        }

        [Theory]
        [InlineData(1000, 100)]
        [InlineData(10000, 1000)]
        [InlineData(100000, 10000)]
        public async Task ReturnsNumberOfLocationsExpected(int maxDistance, int maxResults)
        {
            var listLocationRequest = GetLocationRequest(maxDistance, maxResults);

            var response = await Client.GetAsync(getCallUrl(listLocationRequest));
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = stringResponse.FromJson<ListLocationResponse>();

            Assert.True(model.Locations.Count() > maxResults / 2);
            Assert.True(model.Locations.Count() <= maxResults);
        }


        private ListLocationRequest GetLocationRequest(int maxDistance, int maxResults)
        {
            return new ListLocationRequest
            {
                Location = new LocationRequestDto(50.8440597, 5.7277659),
                MaxDistance = maxDistance,
                MaxResults = maxResults,
            };
        }

        private string getCallUrl(ListLocationRequest listLocationRequest)
        {
            NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
            numberFormatInfo.NumberDecimalSeparator = ".";

            return $"api/getlocations?Location.Latitude={listLocationRequest.Location?.Latitude.ToString(numberFormatInfo)}&Location.Longitude={listLocationRequest.Location?.Longitude.ToString(numberFormatInfo)}&{nameof(listLocationRequest.MaxDistance)}={listLocationRequest.MaxDistance}&{nameof(listLocationRequest.MaxResults)}={listLocationRequest.MaxResults}";
        }
    }
}
