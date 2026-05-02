using Microsoft.AspNetCore.Mvc;
using WecWeatherTracker.Application.UseCases;
using WecWeatherTracker.Domain.Interfaces;

namespace WecWeatherTracker.Api.Controllers;

/// <summary>
/// Expõe os dados climáticos dos circuitos WEC via REST.
/// Para atualizações em tempo real, utilize o hub SignalR em /hubs/weather.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class WeatherController : ControllerBase
{
    private readonly GetLatestWeatherUseCase _getLatestWeatherUseCase;
    private readonly IWeatherRepository _weatherRepository;

    public WeatherController(
        GetLatestWeatherUseCase getLatestWeatherUseCase,
        IWeatherRepository weatherRepository)
    {
        _getLatestWeatherUseCase = getLatestWeatherUseCase;
        _weatherRepository = weatherRepository;
    }

    /// <summary>
    /// Retorna o snapshot climático mais recente de todos os circuitos.
    /// Resultado servido do cache Redis quando disponível.
    /// </summary>
    [HttpGet("latest")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLatestAll(CancellationToken cancellationToken)
    {
        var snapshots = await _getLatestWeatherUseCase.ExecuteAsync(cancellationToken);
        return Ok(snapshots);
    }

    /// <summary>
    /// Retorna os últimos N snapshots de um circuito específico (histórico).
    /// </summary>
    [HttpGet("{circuitId}/history")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistory(
        string circuitId,
        [FromQuery] int limit = 10,
        CancellationToken cancellationToken = default)
    {
        var snapshots = await _weatherRepository.GetLatestByCircuitAsync(
            circuitId, limit, cancellationToken);

        return Ok(snapshots);
    }
}
