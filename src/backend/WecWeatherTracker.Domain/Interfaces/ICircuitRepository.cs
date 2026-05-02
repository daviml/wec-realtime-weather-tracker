using WecWeatherTracker.Domain.Entities;

namespace WecWeatherTracker.Domain.Interfaces;

/// <summary>
/// Defines read operations for <see cref="Circuit"/> reference data.
/// Circuits are seeded on application startup and treated as immutable.
/// </summary>
public interface ICircuitRepository
{
    /// <summary>
    /// Returns all WEC circuits tracked by the system.
    /// </summary>
    Task<IReadOnlyList<Circuit>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a single circuit by its unique identifier, or null if not found.
    /// </summary>
    Task<Circuit?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
}
