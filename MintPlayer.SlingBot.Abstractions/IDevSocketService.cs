using Microsoft.Extensions.Primitives;

namespace MintPlayer.SlingBot.Abstractions;

public interface IDevSocketService
{
    Task NewSocketClient(SocketClient client);
    Task SendToClients(IDictionary<string, StringValues> headers, string body);
}
