using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Solidaridad.API;
using Solidaridad.Api.IntegrationTests.Common;
using Solidaridad.Api.IntegrationTests.Helpers;
using Solidaridad.Application.Models.WeatherForecast;
using Xunit;

namespace Solidaridad.Api.IntegrationTests.Tests;

public class WeatherForecastEndpointTests
{
    private readonly ApiApplicationFactory<Program> _factory;

    public WeatherForecastEndpointTests()
    {
        _factory = new ApiApplicationFactory<Program>();
    }

    [Fact]
    public async Task Login_Should_Return_User_Information_And_Token()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();

        // Act
        var apiResponse = await client.GetAsync("/api/WeatherForecast");

        // Assert
        var response = await ResponseHelper.GetApiResultAsync<IEnumerable<WeatherForecastResponseModel>>(apiResponse);
        CheckResponse.Succeeded(response);
        response.Result.Should().HaveCount(5);
    }
}
