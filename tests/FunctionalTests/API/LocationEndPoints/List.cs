using LocationSearch.API.LocationEndPoints;
using LocationSearch.ApplicationCore.Extensions;
using System.Linq;
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
        public async Task ReturnsNotFoundLocations()
        {
            int expectedCount = 1;
            var listLocationRequest = new ListLocationRequest
            {
                Location = new LocationRequestDto(0, 0),
                MaxDistance = 100,
                MaxResults = 10,
            };
            var response = await Client.GetAsync(getCallUrl(listLocationRequest));

            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = stringResponse.FromJson<ListLocationResponse>();

            Assert.Equal(expectedCount, model.Locations.Count());
        }


        [Theory]
        [InlineData(1, 4)]
        [InlineData(2, 4)]
        [InlineData(5, 4)]
        public async Task ReturnsSuccessGivenValidNewItemAndAdminUserToken(int maxDistance, int expectedCount)
        {
            var listLocationRequest = GetLocationRequest(maxDistance, 10);
            var response = await Client.GetAsync(getCallUrl(listLocationRequest));
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = stringResponse.FromJson<ListLocationResponse>();

            Assert.Equal(expectedCount, model.Locations.Count());
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
            return $"api/getlocations?Location.Latitude={listLocationRequest.Location.Latitude}&Location.Longitude={listLocationRequest.Location.Longitude}&{nameof(listLocationRequest.MaxDistance)}={listLocationRequest.MaxDistance}&{nameof(listLocationRequest.MaxResults)}={listLocationRequest.MaxResults}";
        }
    }
}
