using Microsoft.AspNetCore.Mvc;
using WecWeatherTracker.Domain.Interfaces;

namespace WecWeatherTracker.Api.Controllers;

/// <summary>
/// Expõe os circuitos WEC disponíveis no sistema.
/// Dados estáticos — populados no seed e raramente alterados.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class CircuitsController : ControllerBase
{
    private readonly ICircuitRepository _circuitRepository;

    public CircuitsController(ICircuitRepository circuitRepository)
    {
        _circuitRepository = circuitRepository;
    }

    /// <summary>
    /// Retorna todos os circuitos WEC cadastrados no sistema.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var circuits = await _circuitRepository.GetAllAsync(cancellationToken);
        return Ok(circuits);
    }

    /// <summary>
    /// Retorna um circuito específico pelo seu identificador.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var circuit = await _circuitRepository.GetByIdAsync(id, cancellationToken);
        return circuit is null ? NotFound() : Ok(circuit);
    }
}
