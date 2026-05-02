namespace WecWeatherTracker.Domain.Entities;

/// <summary>
/// Represents an immutable point-in-time weather reading for a WEC circuit.
/// Stored in MongoDB and broadcast via SignalR to connected clients.
/// </summary>
public sealed class WeatherSnapshot
{
    public string Id { get; init; } = string.Empty;
    public string CircuitId { get; init; } = string.Empty;
    public string CircuitName { get; init; } = string.Empty;

    /// <summary>Temperature in Celsius.</summary>
    public double Temperature { get; init; }

    /// <summary>Feels-like temperature in Celsius.</summary>
    public double FeelsLike { get; init; }

    /// <summary>Relative humidity percentage (0–100).</summary>
    public int Humidity { get; init; }

    /// <summary>Wind speed in km/h.</summary>
    public double WindSpeed { get; init; }

    /// <summary>Wind direction in degrees (0–360).</summary>
    public int WindDirection { get; init; }

    /// <summary>Human-readable weather condition (e.g. "Rain", "Clear").</summary>
    public string Condition { get; init; } = string.Empty;

    /// <summary>WMO weather condition code returned by Open-Meteo.</summary>
    public int WeatherCode { get; init; }

    /// <summary>UTC timestamp when this reading was recorded.</summary>
    public DateTime RecordedAt { get; init; }
}
