using System.Net.WebSockets;
using System.Text;

namespace MintPlayer.SlingBot.Abstractions;

public class SocketClient
{
    public SocketClient(WebSocket webSocket) => WebSocket = webSocket;

    public WebSocket WebSocket { get; }

    const int bufferSize = 2048;

    public async Task SendMessage(string message)
    {
        switch (WebSocket.State)
        {
            case WebSocketState.Open:
                var bytes = Encoding.UTF8.GetBytes(message);
                var bytesSent = 0;

                do
                {
                    var arraySegment = new ArraySegment<byte>(bytes, bytesSent, bufferSize);
                    bytesSent += bufferSize;
                    await WebSocket.SendAsync(arraySegment, WebSocketMessageType.Text, bytesSent >= bytes.Length, CancellationToken.None);
                }
                while (bytesSent < bytes.Length);

                break;
            case WebSocketState.Closed:
            case WebSocketState.Aborted:
                throw new WebSocketException("The websocket was closed");
        }
    }
}
