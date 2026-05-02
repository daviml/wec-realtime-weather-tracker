using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using WecWeatherTracker.Domain.Entities;
using WecWeatherTracker.Domain.Interfaces;

namespace WecWeatherTracker.Application.UseCases;

/// <summary>
/// Retorna o snapshot climático mais recente de todos os circuitos,
/// aplicando cache Redis para reduzir a carga no MongoDB.
/// Cache TTL: 55 segundos (polling acontece a cada 60s — dado nunca fica obsoleto).
/// </summary>
public sealed class GetLatestWeatherUseCase
{
    private const string CacheKey = "weather:latest:all";
    private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(55);

    private readonly IWeatherRepository _weatherRepository;
    private readonly IDistributedCache _cache;
    private readonly ILogger<GetLatestWeatherUseCase> _logger;

    public GetLatestWeatherUseCase(
        IWeatherRepository weatherRepository,
        IDistributedCache cache,
        ILogger<GetLatestWeatherUseCase> logger)
    {
        _weatherRepository = weatherRepository;
        _cache = cache;
        _logger = logger;
    }

    public async Task<IReadOnlyList<WeatherSnapshot>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        // 1. Tenta buscar do cache Redis
        var cached = await _cache.GetAsync(CacheKey, cancellationToken);
        if (cached is not null)
        {
            _logger.LogDebug("Cache hit para '{CacheKey}'.", CacheKey);
            return JsonSerializer.Deserialize<List<WeatherSnapshot>>(cached)
                   ?? [];
        }

        // 2. Cache miss — busca no MongoDB
        _logger.LogDebug("Cache miss para '{CacheKey}'. Consultando MongoDB.", CacheKey);
        var snapshots = await _weatherRepository.GetLatestForAllCircuitsAsync(cancellationToken);

        // 3. Armazena no cache Redis
        if (snapshots.Count > 0)
        {
            var serialized = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(snapshots));
            await _cache.SetAsync(CacheKey, serialized,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = CacheTtl },
                cancellationToken);
        }

        return snapshots;
    }

    /// <summary>
    /// Invalida o cache ao receber um novo snapshot.
    /// Chamado pelo <see cref="BackgroundServices.WeatherPollingService"/> após cada atualização.
    /// </summary>
    public async Task InvalidateCacheAsync(CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(CacheKey, cancellationToken);
        _logger.LogDebug("Cache '{CacheKey}' invalidado.", CacheKey);
    }
}
