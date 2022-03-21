using System;
using System.Threading;
using ExitGames.Client.Photon;
using MyMmo.Client;
using MyMmo.Client.Params;
using MyMmo.Commons.Scripts;

namespace MyMmo.ConsolePlayTest {
    public class ConsolePlayTest : IGameListener {

        private readonly Game game;

        private string nickname;
        private bool connected;
        private const string WorldName = "UnityWorld";

        public static void Main(string[] args) {
            var playTest = new ConsolePlayTest();
            playTest.Start();
        }

        private ConsolePlayTest() {
            game = new Game(this);
        }

        private void Start() {
            var thread = new Thread(RunGameLoop);
            thread.IsBackground = true;
            thread.Start();

            Console.WriteLine("Started, press any key to begin");
            TryConnectManually();
            RunUI();
        }

        private void TryConnectManually() {
            //ConnectNgrokWebSocket(); // ngrok
            //ConnectLocalhostWebSocket(); //localhost websocket
            ConnectLocalhostTcp(); // localhost tcp
        }

        private void ConnectNgrokWebSocket() {
            game.Initialize(new PhotonPeer(game, ConnectionProtocol.WebSocket));
            game.Connect("ws://0256-62-122-202-232.ngrok.io:80");
        }

        private void ConnectLocalhostWebSocket() {
            game.Initialize(new PhotonPeer(game, ConnectionProtocol.WebSocket));
            game.Connect("ws://localhost:9090");
        }

        private void ConnectLocalhostTcp() {
            game.Initialize(new PhotonPeer(game, ConnectionProtocol.Tcp));
            game.Connect("localhost:4530");
        }

        private void RunGameLoop() {
            while (true) {
                game.Update();
            }
        }

        private void RunUI() {
            while (true) {
                Console.ReadLine();
                Console.Clear();
                PrintUI();

                Console.WriteLine("-c [address] uri address of server with schema");
                Console.WriteLine("-e [nickname] create default world and enter it with nickname specified");
                Console.WriteLine("-m [locationId] move avatar to location specified");
                Console.WriteLine("-p change item position randomly in the same location");
                Console.WriteLine("Enter command:");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) {
                    PrintLog("nothing..");
                    continue;
                }

                var inputArg = input.Split(' ');
                switch (inputArg[0]) {
                    case "-c": {
                        var address = inputArg[1];
                        Console.WriteLine("input address: " + address);
                        
                        var uri = new Uri(address);
                        if (uri.Scheme.Equals("ws")) {
                            game.Initialize(new PhotonPeer(game, ConnectionProtocol.WebSocket));
                        } else if (uri.Scheme.Equals("tcp")) {
                            game.Initialize(new PhotonPeer(game, ConnectionProtocol.Tcp));
                        } else {
                            PrintLog("uri.Schema is empty, init as tcp");
                        }
                        
                        game.Initialize(new PhotonPeer(game, ConnectionProtocol.Tcp));
                        game.Connect(address);
                        break;
                    }
                    case "-e": {
                        if (!connected) {
                            continue;
                        }
                        nickname = inputArg[1];
                        game.CreateWorld(new CreateWorldParams {WorldName = WorldName});
                        break;
                    }

                    case "-m": {
                        if (!connected) {
                            continue;
                        }
                        var locationId = int.Parse(inputArg[1]);
                        game.ChangeLocation(game.AvatarItem.Id, locationId);
                        break;
                    }

                    case "-p": {
                        if (!connected) {
                            continue;
                        }
                        game.MoveAvatarRandomly();
                        break;
                    }
                }
            }
        }

        private void PrintUI() {
            if (!connected) {
                Console.WriteLine("[not connected]");
                return;
            }
            
            Console.WriteLine($"----------- connected");
            foreach (var item in game.Items) {
                Console.WriteLine($"+item id={item.Id} location={item.LocationId}");
            }

            Console.WriteLine("-----------");
        }

        private static void PrintLog(string message) {
            Console.WriteLine(message + "  >> press any key to continue <<");
        }

        public void OnConnected() {
            PrintLog("connected to server..");
            connected = true;
        }

        public void OnWorldCreated() {
            PrintLog("WorldCreated...");
            game.EnterWorld(new EnterWorldParams {WorldName = WorldName, UserName = nickname});
        }

        public void OnWorldEntered() {
            PrintLog("WorldEntered...");
        }

        public void OnItemSubscribed(Item item) {
            PrintLog($"item {item.Id} entered in our interest area...");
        }

        public void OnItemUnsubscribed(Item item) {
            PrintLog($"item {item} exited from our interest area...");
        }

        public void OnItemDestroyed(Item item) {
            PrintLog($"item {item} is destroyed...");
        }

        public void OnItemLocationChanged(Item item) {
            PrintLog($"item {item.Id} changed its location -> {item.LocationId}");
        }

        void IGameListener.OnLog(DebugLevel debugLevel, string message) {
            PrintLog($"Game Log: {message}");
        }

        public void OnRegionUpdate(int locationId, BaseScriptData[] scriptsData) {
            PrintLog($"region {locationId} updates with scripts");
            foreach (var scriptData in scriptsData) {
                if (scriptData is ChangeLocationScriptData data) {
                    PrintLog($"item {data.ItemId} changes location from {data.FromLocation} to {data.ToLocation}");
                }
                
            }
        }

    }
}