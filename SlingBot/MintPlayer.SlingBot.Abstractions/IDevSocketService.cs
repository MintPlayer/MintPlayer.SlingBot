using Microsoft.Extensions.Primitives;

namespace MintPlayer.SlingBot.Abstractions;

public class Message
{
    public string? Content { get; set; }
    public int Counter { get; set; }
}

public interface IDevSocketService
{
    //Task<Message> GetMessage();
    Task NewSocketClient(SocketClient client);
    Task SendToClients(IDictionary<string, StringValues> headers, string body);
}
