using Microsoft.AspNetCore.SignalR;

namespace WecWeatherTracker.Application.Hubs;

/// <summary>
/// Hub SignalR para comunicação em tempo real com o front-end.
/// Clientes se juntam a grupos por circuito e recebem atualizações climáticas
/// apenas do circuito que estão visualizando.
/// </summary>
public sealed class WeatherHub : Hub
{
    /// <summary>
    /// Adiciona o cliente ao grupo do circuito selecionado.
    /// Chamado pelo front-end ao abrir o detalhe de um circuito.
    /// </summary>
    public async Task JoinCircuit(string circuitId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(circuitId));
    }

    /// <summary>
    /// Remove o cliente do grupo do circuito.
    /// Chamado ao sair da tela de detalhe ou ao trocar de circuito.
    /// </summary>
    public async Task LeaveCircuit(string circuitId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(circuitId));
    }

    /// <summary>
    /// Prefixo padronizado para nomes de grupos, evitando colisões.
    /// </summary>
    public static string GetGroupName(string circuitId) => $"circuit:{circuitId}";
}
