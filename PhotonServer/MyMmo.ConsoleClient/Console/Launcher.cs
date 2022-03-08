using ExitGames.Client.Photon;

namespace MyMmo.ConsoleClient {
    
    internal static class Launcher {
        
        public static void Main(string[] args) {
            var engine = new ConsoleClient();
            var game = new Game(engine);
            var peer = new PhotonPeer(game, ConnectionProtocol.Tcp);
            game.Initialize(peer);
            engine.Initialize(game);
            engine.Run();
        }
    }
}