using Microsoft.Extensions.DependencyInjection;
using WecWeatherTracker.Application.BackgroundServices;
using WecWeatherTracker.Application.UseCases;

namespace WecWeatherTracker.Application;

/// <summary>
/// Extension method que registra todos os serviços da camada Application no DI container.
/// Chamado em <c>Program.cs</c> junto com <c>AddInfrastructure()</c>.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Use Cases como Scoped — instância por requisição HTTP
        services.AddScoped<GetLatestWeatherUseCase>();

        // Worker de polling como Singleton — roda durante toda a vida da aplicação
        services.AddHostedService<WeatherPollingService>();

        return services;
    }
}
