using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WecWeatherTracker.Application.Hubs;
using WecWeatherTracker.Application.UseCases;
using WecWeatherTracker.Domain.Interfaces;

namespace WecWeatherTracker.Application.BackgroundServices;

/// <summary>
/// Worker de background que busca o clima de todos os circuitos a cada 60 segundos,
/// persiste os dados no MongoDB, invalida o cache Redis e transmite via SignalR.
/// </summary>
public sealed class WeatherPollingService : BackgroundService
{
    private static readonly TimeSpan PollingInterval = TimeSpan.FromSeconds(60);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHubContext<WeatherHub> _hubContext;
    private readonly ILogger<WeatherPollingService> _logger;

    public WeatherPollingService(
        IServiceScopeFactory scopeFactory,
        IHubContext<WeatherHub> hubContext,
        ILogger<WeatherPollingService> logger)
    {
        _scopeFactory = scopeFactory;
        _hubContext = hubContext;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("WeatherPollingService iniciado. Intervalo: {Interval}s.", PollingInterval.TotalSeconds);

        // Executa imediatamente na primeira vez para popular o banco no startup
        await PollAllCircuitsAsync(stoppingToken);

        using var timer = new PeriodicTimer(PollingInterval);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await PollAllCircuitsAsync(stoppingToken);
        }
    }

    private async Task PollAllCircuitsAsync(CancellationToken cancellationToken)
    {
        // Scoped porque IWeatherRepository e ICircuitRepository são Scoped
        await using var scope = _scopeFactory.CreateAsyncScope();

        var circuitRepository = scope.ServiceProvider.GetRequiredService<ICircuitRepository>();
        var weatherRepository = scope.ServiceProvider.GetRequiredService<IWeatherRepository>();
        var weatherService = scope.ServiceProvider.GetRequiredService<IWeatherService>();
        var latestWeatherUseCase = scope.ServiceProvider.GetRequiredService<GetLatestWeatherUseCase>();

        var circuits = await circuitRepository.GetAllAsync(cancellationToken);

        _logger.LogInformation("Iniciando polling de clima para {Count} circuitos.", circuits.Count);

        var tasks = circuits.Select(circuit => ProcessCircuitAsync(
            circuit, weatherService, weatherRepository, cancellationToken));

        await Task.WhenAll(tasks);

        // Invalida o cache após atualizar todos os circuitos
        await latestWeatherUseCase.InvalidateCacheAsync(cancellationToken);

        _logger.LogInformation("Polling concluído para todos os circuitos.");
    }

    private async Task ProcessCircuitAsync(
        Domain.Entities.Circuit circuit,
        IWeatherService weatherService,
        IWeatherRepository weatherRepository,
        CancellationToken cancellationToken)
    {
        try
        {
            var snapshot = await weatherService.FetchCurrentWeatherAsync(circuit, cancellationToken);

            await weatherRepository.SaveAsync(snapshot, cancellationToken);

            // Transmite o novo snapshot via SignalR para o grupo do circuito
            await _hubContext.Clients
                .Group(WeatherHub.GetGroupName(circuit.Id))
                .SendAsync("WeatherUpdated", snapshot, cancellationToken);

            _logger.LogDebug("Clima atualizado para {Circuit}: {Temp}°C, {Condition}.",
                circuit.Name, snapshot.Temperature, snapshot.Condition);
        }
        catch (Exception ex)
        {
            // Falha de um circuito não deve interromper o polling dos demais
            _logger.LogError(ex, "Erro ao processar clima do circuito {Circuit}.", circuit.Name);
        }
    }
}
