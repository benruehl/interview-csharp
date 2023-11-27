using System.Net;
using System.Text;
using HashidsNet;

namespace UrlShortenerService.Api.Tests.Endpoints.Url;

[TestClass]
public class RedirectToUrlEndpointTests
{
    private readonly WebApplicationFactory<Program> _factory = new();
    
    [TestMethod]
    public async Task Get_ShouldReturnRedirect_WhenShortIdExists()
    {
        // Arrange
        var insertContent = new StringContent("""{"url": "https://google.com/"}""", Encoding.UTF8, "application/json");
        using var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
 
        // Act
        var insertResponse = await client.PostAsync("/u", insertContent);
        var shortId = (await insertResponse.Content.ReadAsStringAsync()).Trim('"');
        var queryResponse = await client.GetAsync($"/u/{shortId}");
        
        // Assert
        queryResponse.StatusCode.Should().Be(HttpStatusCode.Redirect);
    }
    
    [TestMethod]
    public async Task Get_ShouldReturnNotFound_WhenShortIdDoesNotExist()
    {
        // Arrange
        var nonExistingId = new Hashids().Encode(1);
        using var client = _factory.CreateClient();
 
        // Act
        var response = await client.GetAsync($"/u/{nonExistingId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [TestMethod]
    public async Task Get_ShouldReturnNotFound_WhenShortIdIsInvalid()
    {
        // Arrange
        const string InvalidId = "anyArbitraryId";
        using var client = _factory.CreateClient();
 
        // Act
        var response = await client.GetAsync($"/u/{InvalidId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
