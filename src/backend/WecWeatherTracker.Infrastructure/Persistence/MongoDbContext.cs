using MongoDB.Driver;
using WecWeatherTracker.Domain.Entities;

namespace WecWeatherTracker.Infrastructure.Persistence;

/// <summary>
/// Encapsula o acesso ao banco MongoDB, expondo as coleções tipadas.
/// Registrado como Singleton para reutilizar a conexão durante toda a vida da aplicação.
/// </summary>
public sealed class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IMongoClient client, string databaseName)
    {
        _database = client.GetDatabase(databaseName);
        EnsureIndexes();
    }

    public IMongoCollection<WeatherSnapshot> WeatherSnapshots =>
        _database.GetCollection<WeatherSnapshot>("weather_snapshots");

    public IMongoCollection<Circuit> Circuits =>
        _database.GetCollection<Circuit>("circuits");

    /// <summary>
    /// Cria índices necessários para performance das queries mais frequentes.
    /// Chamado apenas uma vez na inicialização.
    /// </summary>
    private void EnsureIndexes()
    {
        // Índice para buscar snapshots por circuito ordenados por data (query mais comum)
        var snapshotIndexKeys = Builders<WeatherSnapshot>.IndexKeys
            .Ascending(s => s.CircuitId)
            .Descending(s => s.RecordedAt);

        var snapshotIndexModel = new CreateIndexModel<WeatherSnapshot>(
            snapshotIndexKeys,
            new CreateIndexOptions { Name = "idx_circuit_recorded" });

        WeatherSnapshots.Indexes.CreateOne(snapshotIndexModel);
    }
}
