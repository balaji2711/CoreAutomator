using System.Net.WebSockets;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CoreAutomator.Action
{
    public class WebsocketActions
    {
        public ClientWebSocket client;
        CancellationTokenSource cancellationTokenSource;

        public void OpenWebSocketWithHeader(string subProtocol)
        {
            client = new ClientWebSocket();
            client.Options.AddSubProtocol(subProtocol);
        }

        public void OpenWebSocketWithoutHeader()
        {
            client = new ClientWebSocket();
        }

        public WebSocketState State()
        {
            return client.State;
        }

        public async Task Connect(Uri uri)
        {
            await client.ConnectAsync(uri, CancellationToken.None);
        }

        public ArraySegment<byte> Send(string xml, string messageType, bool endOfMessage, CancellationTokenSource cancellationTokenSource)
        {
            ArraySegment<byte> bytesToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(xml));
            if (messageType == "Text")
                client.SendAsync(bytesToSend, WebSocketMessageType.Text, endOfMessage, cancellationTokenSource.Token);
            else if (messageType == "Binary")
                client.SendAsync(bytesToSend, WebSocketMessageType.Binary, endOfMessage, cancellationTokenSource.Token);
            return bytesToSend;
        }

        public async Task<WebSocketReceiveResult> Receive(ArraySegment<byte> bytesReceived, CancellationTokenSource cancellationTokenSource)
        {
            WebSocketReceiveResult result = await client.ReceiveAsync(bytesReceived, cancellationTokenSource.Token);
            return result;
        }

        public async Task Close()
        {
            await client.CloseAsync(WebSocketCloseStatus.NormalClosure, null, default);
        }

        public async void Delay(int milliseconds)
        {
            await Task.Delay(milliseconds, cancellationTokenSource.Token);
        }

        public string SerializeXML<T>(T obj, string prefix, string ns, string attachOneMoreNamespace)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            if (attachOneMoreNamespace == "asajj")
                namespaces.Add("asajj", "http://IntelligentGaming/Asajj/v1.0");
            else
                namespaces.Add("lara", "http://IntelligentGaming/Lara/v1.0");
            namespaces.Add(prefix, ns);
            using (var stringWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(stringWriter))
                {
                    xsSubmit.Serialize(writer, obj, namespaces);
                    return stringWriter.ToString();
                }
            }
        }
        //Reference: https://github.com/paulbatum/WebSocket-Samples/blob/master/HttpListenerWebSocketEcho/Client/Client.cs
    }
}