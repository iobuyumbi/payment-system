using Solidaridad.Application.Models.WeatherForecast;

namespace Solidaridad.Application.Services;

public interface IWeatherForecastService
{
    public Task<IEnumerable<WeatherForecastResponseModel>> GetAsync();
}
