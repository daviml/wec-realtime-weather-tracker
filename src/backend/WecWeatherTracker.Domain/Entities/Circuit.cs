namespace WecWeatherTracker.Domain.Entities;

/// <summary>
/// Represents a WEC racing circuit with geolocation data
/// used to fetch real-time weather conditions.
/// </summary>
public sealed class Circuit
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public double Latitude { get; init; }
    public double Longitude { get; init; }
}
