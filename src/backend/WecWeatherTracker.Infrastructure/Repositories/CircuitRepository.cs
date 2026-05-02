using MongoDB.Driver;
using WecWeatherTracker.Domain.Entities;
using WecWeatherTracker.Domain.Interfaces;
using WecWeatherTracker.Infrastructure.Persistence;

namespace WecWeatherTracker.Infrastructure.Repositories;

/// <summary>
/// Implementação MongoDB de <see cref="ICircuitRepository"/>.
/// Os circuitos são dados de referência — imutáveis em runtime.
/// </summary>
public sealed class CircuitRepository : ICircuitRepository
{
    private readonly IMongoCollection<Circuit> _collection;

    public CircuitRepository(MongoDbContext context)
    {
        _collection = context.Circuits;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Circuit>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _collection
            .Find(_ => true)
            .SortBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Circuit?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _collection
            .Find(c => c.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Inserção utilizada apenas pelo <see cref="Seed.CircuitSeeder"/> na inicialização.
    /// </summary>
    internal async Task InsertManyAsync(IEnumerable<Circuit> circuits, CancellationToken cancellationToken = default)
    {
        await _collection.InsertManyAsync(circuits, cancellationToken: cancellationToken);
    }

    internal async Task<long> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _collection.CountDocumentsAsync(_ => true, cancellationToken: cancellationToken);
    }
}
