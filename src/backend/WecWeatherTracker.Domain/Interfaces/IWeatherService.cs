using WecWeatherTracker.Domain.Entities;

namespace WecWeatherTracker.Domain.Interfaces;

/// <summary>
/// Abstracts the external weather data provider.
/// Implementations in the Infrastructure layer handle HTTP communication
/// with third-party APIs (e.g. Open-Meteo) and map responses to domain entities.
/// </summary>
public interface IWeatherService
{
    /// <summary>
    /// Fetches the current weather conditions for the given circuit
    /// using its geographic coordinates.
    /// </summary>
    /// <param name="circuit">The circuit to fetch weather for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A new <see cref="WeatherSnapshot"/> with the current conditions.</returns>
    Task<WeatherSnapshot> FetchCurrentWeatherAsync(
        Circuit circuit,
        CancellationToken cancellationToken = default);
}
