using System.Threading;
using MyMmo.Client.Params;

namespace MyMmo.Client.Console {
    public class ConsoleClient : IGameListener {

        private Game game;

        private string nickname;
        private bool connected;

        public void Initialize(Game gameInstance) {
            game = gameInstance;
        }

        public void Run() {
            game.Connect();
            
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
                        game.CreateWorld(new CreateWorldParams {WorldName = "FirstWorld"});
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
            game.EnterWorld(new EnterWorldParams {WorldName = "FirstWorld", UserName = nickname});
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

    }
}