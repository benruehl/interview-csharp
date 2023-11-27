using System.Net;
using System.Text;

namespace UrlShortenerService.Api.Tests.Endpoints.Url;

[TestClass]
public class CreateShortUrlEndpointTests
{
    private readonly WebApplicationFactory<Program> _factory = new();
    
    [TestMethod]
    public async Task Post_ShouldReturnOk_WhenValidUrlIsGiven()
    {
        // Arrange
        var requestBody = new StringContent("""{"url": "https://google.com/"}""", Encoding.UTF8, "application/json");
        using var client = _factory.CreateClient();
 
        // Act
        var response = await client.PostAsync("/u", requestBody);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [TestMethod]
    public async Task Post_ShouldReturnBadRequest_WhenInvalidUrlIsGiven()
    {
        // Arrange
        var requestBody = new StringContent("{\"url\": \"anyInvalidUrl\"}", Encoding.UTF8, "application/json");
        using var client = _factory.CreateClient();
 
        // Act
        var response = await client.PostAsync("/u", requestBody);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
