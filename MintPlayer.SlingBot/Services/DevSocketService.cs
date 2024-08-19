using Microsoft.Extensions.Primitives;
using MintPlayer.SlingBot.Abstractions;
using System.Net.WebSockets;

namespace MintPlayer.SlingBot.Services;

internal class DevSocketService : IDevSocketService
{
    private readonly List<SocketClient> clients = new List<SocketClient>();
    public DevSocketService()
    {
    }

    private int counter = 1;
    public Task<Message> GetMessage()
    {
        return Task.FromResult(new Message { Content = "Some message from the server", Counter = counter++ });
    }

    public Task NewSocketClient(SocketClient client)
    {
        clients.Add(client);
        // TODO: On close remove from list

        return Task.CompletedTask;
    }

    public async Task SendToClients(IDictionary<string, StringValues> headers, string body)
    {
        var payload = $"""
            {string.Join(Environment.NewLine, headers.Select(h => $"{h.Key}: {h.Value}"))}

            {body}
            """;

        await Task.WhenAll(clients.Where(c => c.WebSocket.State == WebSocketState.Open).Select(c => c.SendMessage(payload)));
    }
}
