using ExitGames.Client.Photon;
using MyMmo.Client;

namespace MyMmo.ConsolePlayTest {
    
    internal static class Launcher {
        
        public static void Main(string[] args) {
            var engine = new ConsolePlayTest();
            var game = new Game(engine);
            var peer = new PhotonPeer(game, ConnectionProtocol.Tcp);
            game.Initialize(peer);
            engine.Initialize(game);
            engine.Run();
        }
    }
}