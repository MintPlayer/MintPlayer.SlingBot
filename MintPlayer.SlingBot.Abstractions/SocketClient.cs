using System.Net.WebSockets;
using System.Text;

namespace MintPlayer.SlingBot.Abstractions;

public class SocketClient
{
    public SocketClient(WebSocket webSocket, string email)
    {
        WebSocket = webSocket;
        Email = email;
    }

    public WebSocket WebSocket { get; }
    public string Email { get; }

    const int bufferSize = 2048;

    public async Task SendMessage(string message)
    {
        switch (WebSocket.State)
        {
            case WebSocketState.Open:
                await WebSocket.WriteMessage(message);
                break;
            case WebSocketState.Closed:
            case WebSocketState.Aborted:
                throw new WebSocketException("The websocket was closed");
        }
    }
}
