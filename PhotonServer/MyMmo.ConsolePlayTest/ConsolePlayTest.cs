using System.Threading;
using ExitGames.Client.Photon;
using MyMmo.Client;
using MyMmo.Client.Params;

namespace MyMmo.ConsolePlayTest {
    public class ConsolePlayTest : IGameListener {

        private readonly Game game;

        private string nickname;
        private bool connected;
        private const string WorldName = "UnityWorld";

        public static void Main(string[] args) {
            var playTest = new ConsolePlayTest(ConnectionProtocol.Tcp);
            playTest.Start("localhost:4530");
        }

        private ConsolePlayTest(ConnectionProtocol connectionProtocol) {
            game = new Game(this);
            game.Initialize(new PhotonPeer(game, connectionProtocol));
        }

        private void Start(string address) {
            game.Connect(address);
            
            var thread = new Thread(RunGameLoop);
            thread.IsBackground = true;
            thread.Start();

            RunUI();
        }

        private void RunGameLoop() {
            while (true) {
                game.Update();
            }
        }

        private void RunUI() {
            while (true) {
                if (!connected) {
                    continue;
                }

                System.Console.ReadLine();
                System.Console.Clear();
                PrintUI();

                System.Console.WriteLine("-e [nickname] create default world and enter it with nickname specified");
                System.Console.WriteLine("-m [locationId] move avatar to location specified");
                System.Console.WriteLine("Enter command:");
                var input = System.Console.ReadLine();
                if (string.IsNullOrEmpty(input)) {
                    PrintLog("nothing..");
                    continue;
                }

                var inputArg = input.Split(' ');
                switch (inputArg[0]) {
                    case "-e": {
                        nickname = inputArg[1];
                        game.CreateWorld(new CreateWorldParams {WorldName = WorldName});
                        break;
                    }

                    case "-m": {
                        var locationId = int.Parse(inputArg[1]);
                        game.ChangeLocation(game.AvatarItem.Id, locationId);
                        break;
                    }
                }
            }
        }

        private void PrintUI() {
            System.Console.WriteLine("-----------");
            foreach (var item in game.Items) {
                System.Console.WriteLine($"+item id={item.Id} location={item.LocationId}");
            }

            System.Console.WriteLine("-----------");
        }

        private static void PrintLog(string message) {
            System.Console.WriteLine(message + "  >> press any key to continue <<");
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

        public void OnItemEnter(Item item) {
            PrintLog($"item {item.Id} entered in our interest area...");
        }

        public void OnItemExit(string itemId) {
            PrintLog($"item {itemId} exited from our interest area...");
        }

        public void OnItemLocationChanged(Item item) {
            PrintLog($"item {item.Id} changed its location -> {item.LocationId}");
        }

        void IGameListener.OnLog(DebugLevel debugLevel, string message) {
            PrintLog($"Game Log: {message}");
        }
    }
}