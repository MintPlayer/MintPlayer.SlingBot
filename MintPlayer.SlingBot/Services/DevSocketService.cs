using Microsoft.Extensions.Primitives;
using MintPlayer.SlingBot.Abstractions;
using MintPlayer.SlingBot.Extensions;
using System.Net.WebSockets;

namespace MintPlayer.SlingBot.Services;

internal class DevSocketService : IDevSocketService
{
    private readonly List<SocketClient> clients = new List<SocketClient>();
    public DevSocketService()
    {
    }

    public async Task NewSocketClient(SocketClient client)
    {
        clients.Add(client);

        while (true)
        {
            // Keep websocket connection open
            await Task.Delay(1000);
            if (client.WebSocket.State is WebSocketState.CloseReceived or WebSocketState.Closed)
            {
                break;
            }
        }

        //clients.Remove(client);
    }

    public async Task SendToClients(IDictionary<string, StringValues> headers, string body)
    {
        var payload = $"""
            {string.Join(Environment.NewLine, headers.Select(h => $"{h.Key}: {h.Value}"))}

            {body}
            """;

        //await Task.WhenAll(clients
        //    .Where(c => c.WebSocket.State == WebSocketState.Open)
        //    .Select(c => c.SendMessage(payload))
        //);
        foreach (var client in clients)
        {
            if (client.WebSocket.State == WebSocketState.Open)
            {
                await client.SendMessage(payload);
            }
        }
    }
}
