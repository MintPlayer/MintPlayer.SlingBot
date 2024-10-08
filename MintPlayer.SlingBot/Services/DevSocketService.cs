﻿using Microsoft.Extensions.Primitives;
using MintPlayer.SlingBot.Abstractions;
using MintPlayer.SlingBot.Extensions;
using Octokit.Webhooks;
using System.Net.WebSockets;

namespace MintPlayer.SlingBot.Services;

internal class DevSocketService : IDevSocketService
{
    private readonly List<SocketClient> clients = new List<SocketClient>();
    private readonly SlingBotWebhookEventProcessor processor;
    public DevSocketService(SlingBotWebhookEventProcessor processor)
    {
        this.processor = processor;
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

        clients.Remove(client);
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

        var headersObj = WebhookHeaders.Parse(headers);
        var hook = processor.DeserializeWebhookEvent(headersObj, body);

        var allowedClients = await processor.GetDevSocketsForWebhook(hook, clients);

        foreach (var client in allowedClients)
        {
            if (client.WebSocket.State == WebSocketState.Open)
            {
                await client.SendMessage(payload);
            }
        }
    }
}
