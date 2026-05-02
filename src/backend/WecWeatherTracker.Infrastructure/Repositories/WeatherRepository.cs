using MongoDB.Driver;
using WecWeatherTracker.Domain.Entities;
using WecWeatherTracker.Domain.Interfaces;
using WecWeatherTracker.Infrastructure.Persistence;

namespace WecWeatherTracker.Infrastructure.Repositories;

/// <summary>
/// Implementação MongoDB de <see cref="IWeatherRepository"/>.
/// </summary>
public sealed class WeatherRepository : IWeatherRepository
{
    private readonly IMongoCollection<WeatherSnapshot> _collection;

    public WeatherRepository(MongoDbContext context)
    {
        _collection = context.WeatherSnapshots;
    }

    /// <inheritdoc />
    public async Task SaveAsync(WeatherSnapshot snapshot, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(snapshot, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<WeatherSnapshot>> GetLatestByCircuitAsync(
        string circuitId,
        int limit = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _collection
            .Find(s => s.CircuitId == circuitId)
            .SortByDescending(s => s.RecordedAt)
            .Limit(limit)
            .ToListAsync(cancellationToken);

        return result;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<WeatherSnapshot>> GetLatestForAllCircuitsAsync(
        CancellationToken cancellationToken = default)
    {
        // Agrega o snapshot mais recente de cada circuito com um pipeline eficiente
        var pipeline = new[]
        {
            new MongoDB.Bson.BsonDocument("$sort", new MongoDB.Bson.BsonDocument("RecordedAt", -1)),
            new MongoDB.Bson.BsonDocument("$group", new MongoDB.Bson.BsonDocument
            {
                { "_id", "$CircuitId" },
                { "doc", new MongoDB.Bson.BsonDocument("$first", "$$ROOT") }
            }),
            new MongoDB.Bson.BsonDocument("$replaceRoot", new MongoDB.Bson.BsonDocument("newRoot", "$doc"))
        };

        var result = await _collection
            .Aggregate<WeatherSnapshot>(pipeline, cancellationToken: cancellationToken)
            .ToListAsync(cancellationToken);

        return result;
    }
}
