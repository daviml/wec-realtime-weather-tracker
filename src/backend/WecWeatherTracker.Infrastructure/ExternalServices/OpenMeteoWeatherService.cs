using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using WecWeatherTracker.Domain.Entities;
using WecWeatherTracker.Domain.Interfaces;

namespace WecWeatherTracker.Infrastructure.ExternalServices;

/// <summary>
/// Implementação de <see cref="IWeatherService"/> usando a Open-Meteo API.
/// Open-Meteo é gratuita, sem necessidade de API key, e aceita coordenadas lat/lon.
/// Docs: https://open-meteo.com/en/docs
/// </summary>
public sealed class OpenMeteoWeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenMeteoWeatherService> _logger;

    // Mapeamento dos códigos WMO para descrição legível
    // Ref: https://open-meteo.com/en/docs#weathervariables
    private static readonly Dictionary<int, string> WmoConditions = new()
    {
        { 0,  "Céu limpo" },
        { 1,  "Principalmente limpo" },
        { 2,  "Parcialmente nublado" },
        { 3,  "Nublado" },
        { 45, "Neblina" },
        { 48, "Neblina com gelo" },
        { 51, "Garoa fraca" },
        { 53, "Garoa moderada" },
        { 55, "Garoa intensa" },
        { 61, "Chuva fraca" },
        { 63, "Chuva moderada" },
        { 65, "Chuva forte" },
        { 71, "Neve fraca" },
        { 73, "Neve moderada" },
        { 75, "Neve intensa" },
        { 80, "Pancadas de chuva" },
        { 81, "Pancadas moderadas" },
        { 82, "Pancadas fortes" },
        { 95, "Trovoada" },
        { 96, "Trovoada com granizo" },
        { 99, "Trovoada intensa" }
    };

    public OpenMeteoWeatherService(HttpClient httpClient, ILogger<OpenMeteoWeatherService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<WeatherSnapshot> FetchCurrentWeatherAsync(
        Circuit circuit,
        CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(circuit.Latitude, circuit.Longitude);

        _logger.LogDebug("Buscando clima para {Circuit} via Open-Meteo. URL: {Url}", circuit.Name, url);

        var response = await _httpClient.GetFromJsonAsync<OpenMeteoResponse>(url, cancellationToken)
            ?? throw new InvalidOperationException($"Resposta nula da Open-Meteo para o circuito {circuit.Name}");

        var current = response.Current;

        var condition = WmoConditions.GetValueOrDefault(current.WeatherCode, "Desconhecido");

        return new WeatherSnapshot
        {
            Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
            CircuitId = circuit.Id,
            CircuitName = circuit.Name,
            Temperature = current.Temperature,
            FeelsLike = current.ApparentTemperature,
            Humidity = current.RelativeHumidity,
            WindSpeed = current.WindSpeed,
            WindDirection = current.WindDirection,
            Condition = condition,
            WeatherCode = current.WeatherCode,
            RecordedAt = DateTime.UtcNow
        };
    }

    private static string BuildUrl(double latitude, double longitude) =>
        $"https://api.open-meteo.com/v1/forecast" +
        $"?latitude={latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
        $"&longitude={longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
        $"&current=temperature_2m,apparent_temperature,relative_humidity_2m" +
        $",wind_speed_10m,wind_direction_10m,weather_code" +
        $"&wind_speed_unit=kmh" +
        $"&timezone=UTC";

    // DTO interno para desserialização da resposta Open-Meteo
    private sealed record OpenMeteoResponse(OpenMeteoCurrent Current);

    private sealed record OpenMeteoCurrent(
        [property: System.Text.Json.Serialization.JsonPropertyName("temperature_2m")]
        double Temperature,

        [property: System.Text.Json.Serialization.JsonPropertyName("apparent_temperature")]
        double ApparentTemperature,

        [property: System.Text.Json.Serialization.JsonPropertyName("relative_humidity_2m")]
        int RelativeHumidity,

        [property: System.Text.Json.Serialization.JsonPropertyName("wind_speed_10m")]
        double WindSpeed,

        [property: System.Text.Json.Serialization.JsonPropertyName("wind_direction_10m")]
        int WindDirection,

        [property: System.Text.Json.Serialization.JsonPropertyName("weather_code")]
        int WeatherCode
    );
}
