using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using StackExchange.Redis;
using WecWeatherTracker.Domain.Interfaces;
using WecWeatherTracker.Infrastructure.ExternalServices;
using WecWeatherTracker.Infrastructure.Persistence;
using WecWeatherTracker.Infrastructure.Repositories;
using WecWeatherTracker.Infrastructure.Seed;

namespace WecWeatherTracker.Infrastructure;

/// <summary>
/// Extension method que registra todos os serviços da camada Infrastructure no DI container.
/// Chamado uma única vez em <c>Program.cs</c>.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMongoDB(configuration);
        services.AddRedis(configuration);
        services.AddRepositories();
        services.AddWeatherService();

        return services;
    }

    private static void AddMongoDB(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDB")
            ?? "mongodb://localhost:27017";

        var databaseName = configuration["MongoDB:DatabaseName"] ?? "wec_weather";

        services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));

        // MongoDbContext como Singleton: uma instância por aplicação, reutilizando a connection pool
        services.AddSingleton(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return new MongoDbContext(client, databaseName);
        });
    }

    private static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnection = configuration.GetConnectionString("Redis") ?? "localhost:6379";

        // Registra o multiplexer Redis como Singleton (recomendação oficial do StackExchange.Redis)
        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(redisConnection));
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        // CircuitRepository registrado como Scoped para compatibilidade com o seeder
        services.AddScoped<CircuitRepository>();
        services.AddScoped<ICircuitRepository, CircuitRepository>();
        services.AddScoped<IWeatherRepository, WeatherRepository>();

        // Seeder como Scoped — usado apenas no startup
        services.AddScoped<CircuitSeeder>();
    }

    private static void AddWeatherService(this IServiceCollection services)
    {
        // HttpClient com timeout configurado para evitar travamentos no polling
        services.AddHttpClient<IWeatherService, OpenMeteoWeatherService>(client =>
        {
            client.BaseAddress = new Uri("https://api.open-meteo.com");
            client.Timeout = TimeSpan.FromSeconds(10);
        });
    }
}
