using WecWeatherTracker.Domain.Entities;

namespace WecWeatherTracker.Domain.Interfaces;

/// <summary>
/// Defines persistence operations for <see cref="WeatherSnapshot"/> entities.
/// </summary>
public interface IWeatherRepository
{
    /// <summary>
    /// Saves a new weather snapshot to the data store.
    /// </summary>
    Task SaveAsync(WeatherSnapshot snapshot, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the most recent weather snapshots for a given circuit,
    /// ordered by <see cref="WeatherSnapshot.RecordedAt"/> descending.
    /// </summary>
    Task<IReadOnlyList<WeatherSnapshot>> GetLatestByCircuitAsync(
        string circuitId,
        int limit = 10,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the single latest snapshot for every circuit.
    /// Used to populate the dashboard on initial load.
    /// </summary>
    Task<IReadOnlyList<WeatherSnapshot>> GetLatestForAllCircuitsAsync(
        CancellationToken cancellationToken = default);
}
