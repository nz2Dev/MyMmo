using System;
using System.Threading;
using ExitGames.Client.Photon;

namespace V5ConsolePlayTest {
    internal class V5ConsolePlayTest : IPhotonPeerListener {

        private PhotonPeer peer;

        public static void Main(string[] args) {
            var playTest = new V5ConsolePlayTest();
            playTest.Start();
        }

        private void Start() {
            peer = new PhotonPeer(this, ConnectionProtocol.WebSocket);
            var connectResult = peer.Connect("ws://1abe-62-122-202-232.ngrok.io:80", "Master");
            DebugReturn(DebugLevel.INFO, "connect return: " + connectResult);

            if (connectResult) {
                new Thread(() => {
                    while (true) {
                        peer.Service();
                    }
                }).Start();
            }

            Console.ReadLine();
        }

        public void DebugReturn(DebugLevel level, string message) {
            Console.WriteLine($"{level}: {message}");
        }

        public void OnOperationResponse(OperationResponse operationResponse) {
            DebugReturn(DebugLevel.INFO, $"operation response: {operationResponse.ToStringFull()}");
        }

        public void OnStatusChanged(StatusCode statusCode) {
            DebugReturn(DebugLevel.INFO, $"status changed" + statusCode);
        }

        public void OnEvent(EventData eventData) {
            DebugReturn(DebugLevel.INFO, $"event: " + eventData.ToStringFull());
        }

    }
}