using Microsoft.Extensions.Logging;
using WecWeatherTracker.Domain.Entities;
using WecWeatherTracker.Infrastructure.Repositories;

namespace WecWeatherTracker.Infrastructure.Seed;

/// <summary>
/// Popula o MongoDB com os circuitos do WEC na primeira inicialização da aplicação.
/// Executado como parte do startup — idempotente (não duplica dados).
/// </summary>
public sealed class CircuitSeeder
{
    private readonly CircuitRepository _repository;
    private readonly ILogger<CircuitSeeder> _logger;

    public CircuitSeeder(CircuitRepository repository, ILogger<CircuitSeeder> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var count = await _repository.CountAsync(cancellationToken);
        if (count > 0)
        {
            _logger.LogInformation("Circuitos já existem no banco ({Count}). Seed ignorado.", count);
            return;
        }

        _logger.LogInformation("Iniciando seed dos circuitos WEC...");

        var circuits = new List<Circuit>
        {
            new()
            {
                Id = "interlagos",
                Name = "Autódromo José Carlos Pace",
                Location = "Interlagos, São Paulo",
                Country = "Brasil",
                Latitude = -23.7036,
                Longitude = -46.6997
            },
            new()
            {
                Id = "le-mans",
                Name = "Circuit de la Sarthe",
                Location = "Le Mans",
                Country = "França",
                Latitude = 47.9497,
                Longitude = 0.2081
            },
            new()
            {
                Id = "spa",
                Name = "Circuit de Spa-Francorchamps",
                Location = "Stavelot",
                Country = "Bélgica",
                Latitude = 50.4372,
                Longitude = 5.9714
            },
            new()
            {
                Id = "imola",
                Name = "Autodromo Enzo e Dino Ferrari",
                Location = "Imola",
                Country = "Itália",
                Latitude = 44.3439,
                Longitude = 11.7167
            },
            new()
            {
                Id = "cota",
                Name = "Circuit of the Americas",
                Location = "Austin, Texas",
                Country = "Estados Unidos",
                Latitude = 30.1328,
                Longitude = -97.6411
            },
            new()
            {
                Id = "fuji",
                Name = "Fuji Speedway",
                Location = "Oyama, Shizuoka",
                Country = "Japão",
                Latitude = 35.3717,
                Longitude = 138.9253
            },
            new()
            {
                Id = "losail",
                Name = "Losail International Circuit",
                Location = "Lusail",
                Country = "Qatar",
                Latitude = 25.4900,
                Longitude = 51.4542
            },
            new()
            {
                Id = "bahrain",
                Name = "Bahrain International Circuit",
                Location = "Sakhir",
                Country = "Bahrein",
                Latitude = 26.0325,
                Longitude = 50.5106
            }
        };

        await _repository.InsertManyAsync(circuits, cancellationToken);
        _logger.LogInformation("{Count} circuitos inseridos com sucesso.", circuits.Count);
    }
}
