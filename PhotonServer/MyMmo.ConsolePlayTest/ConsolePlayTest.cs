using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ExitGames.Client.Photon;
using MyMmo.Client;
using MyMmo.Client.Params;
using MyMmo.Commons.Scripts;
using MyMmo.Commons.Snapshots;

namespace MyMmo.ConsolePlayTest {
    public class ConsolePlayTest : IGameListener {

        private readonly Game game;

        private string nickname;
        private bool connected;
        private const string WorldName = "UnityWorld";

        private readonly Dictionary<string, ConsoleItem> itemCache = new Dictionary<string, ConsoleItem>();
        private static bool isGameLogEnabled = false;

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
            ConnectLocalhostTcp();
        }

        private void ConnectLocalWebSocketSecure() {
            game.Initialize(new PhotonPeer(game, ConnectionProtocol.WebSocketSecure));
            game.Connect("wss://mymmo.ping.com:9090");
        }

        private void ConnectNgrokWebSocketSecure() {
            game.Initialize(new PhotonPeer(game, ConnectionProtocol.WebSocketSecure));
            game.Connect("wss://a2cd-62-122-202-155.ngrok.io:443");
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
                Console.WriteLine("-l toggle logs on or off");
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

                    case "-l": {
                        isGameLogEnabled = !isGameLogEnabled;
                        Console.WriteLine("Logs is " + (isGameLogEnabled ? "enabled" : "disabled"));
                        break;
                    }

                    case "-m": {
                        if (!connected) {
                            continue;
                        }
                        var locationId = int.Parse(inputArg[1]);
                        game.ChangeLocation(game.AvatarId, locationId);
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
            foreach (var item in itemCache.Values) {
                Console.WriteLine($"+item id={item.ItemId} location={item.LocationId}");
            }

            Console.WriteLine("-----------");
        }
        
        public static void PrintLog(string message) {
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

        public void OnLocationEntered(LocationSnapshotData locationSnapshotData) {
            PrintLog($"On location {locationSnapshotData.LocationId} entered, and we recreate its state representation...");
            var itemsAtLocation = itemCache.Values.Where(item => item.LocationId == locationSnapshotData.LocationId);
            foreach (var item in itemsAtLocation) {
                itemCache.Remove(item.ItemId);
            }
                    
            foreach (var itemSnapshotData in locationSnapshotData.ItemsSnapshotData) {
                itemCache.Add(itemSnapshotData.ItemId, new ConsoleItem(
                    itemSnapshotData
                ));
            }
        }

        public void OnLocationExit(int locationId) {
            PrintLog("On location exit, we won't receive its update for now, and will definetly recreate it if it will enter again");
        }

        public void OnLocationUpdate(int locationId, ScriptsClipData scriptsClipData) {
            PrintLog($"region {locationId} updates with scripts");
            ConsoleScriptClipPlayer.Play(scriptsClipData, itemCache);
        }
        
        void IGameListener.OnLog(DebugLevel debugLevel, string message) {
            if (isGameLogEnabled) {
                PrintLog($"Game Log: {message}");
            }
        }

    }
}