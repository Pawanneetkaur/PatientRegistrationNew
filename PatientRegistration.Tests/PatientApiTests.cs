using Xunit;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace PatientRegistration.Tests.IntegrationTests
{
    public class PatientApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public PatientApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

       [Fact]
        public async Task Ping_ReturnsPong()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/patient/ping");
            var stringContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Now we expect exactly "pong" (no quotes)
            Assert.Equal("pong", stringContent);
        }
    }
}
