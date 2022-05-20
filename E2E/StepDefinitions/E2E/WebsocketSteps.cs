using CoreAutomator.Action;
using NUnit.Framework;
using System.Net.WebSockets;

namespace E2E.StepDefinitions.E2E
{
    [Binding]
    public class WebsocketSteps
    {
        byte[] receiveBuffer;
        static string binaryMessage = null;
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        WebsocketActions websocketActions = new WebsocketActions();

        [When(@"I tests a valid socket connection")]
        public async Task WhenITestsAValidSocketConnection()
        {
            websocketActions.OpenWebSocketWithoutHeader();
            await websocketActions.Connect(new Uri("wss://demo.piesocket.com/v3/channel_1?api_key=VCXCEuvhGcBDP7XhiJJUDvR1e1D3eiVjgZ9VRiaV&notify_self"));
            while (websocketActions.State() == WebSocketState.Open)
            {
                string input = "Test Web Socket";
                ArraySegment<byte> bytesToSend = websocketActions.Send(input, "Text", true, cancellationTokenSource);
                receiveBuffer = new byte[9000];
                var offset = 0;
                var dataPerPacket = 9000;
                while (true)
                {
                    ArraySegment<byte> bytesReceived = new ArraySegment<byte>(receiveBuffer, offset, dataPerPacket);
                    WebSocketReceiveResult result = await websocketActions.Receive(bytesReceived, cancellationTokenSource);
                    binaryMessage = System.Text.Encoding.ASCII.GetString(bytesReceived).Trim();
                    offset += result.Count;
                    if (result.EndOfMessage)
                    {
                        await websocketActions.Close();
                        break;
                    }
                }
            }
        }

        [Then(@"connection should be established")]
        public void ThenConnectionShouldBeEstablished()
        {
            Assert.NotNull(binaryMessage);
            Assert.AreEqual("Test Web Socket", binaryMessage.Replace("\0", string.Empty), "Failed: The message we received is not expected");
        }
    }
}