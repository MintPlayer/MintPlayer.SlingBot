using System.Net.WebSockets;
using System.Text;

namespace MintPlayer.SlingBot.Abstractions;

public class SocketClient
{
    public SocketClient(WebSocket webSocket) => WebSocket = webSocket;

    public WebSocket WebSocket { get; }

    public async Task SendMessage(string message)
    {
        switch (WebSocket.State)
        {
            case WebSocketState.Open:
                var bytes = Encoding.UTF8.GetBytes(message);
                var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                await WebSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                break;
            case WebSocketState.Closed:
            case WebSocketState.Aborted:
                throw new WebSocketException("The websocket was closed");
        }
    }
}
