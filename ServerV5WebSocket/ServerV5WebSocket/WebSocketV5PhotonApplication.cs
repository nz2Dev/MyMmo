using System.IO;
using ExitGames.Logging.Log4Net;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using Photon.SocketServer;
using LogManager = ExitGames.Logging.LogManager;

namespace ServerV5WebSocket {
    public class WebSocketV5PhotonApplication : ApplicationBase {

        static WebSocketV5PhotonApplication() {
            LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
        }
        
        public WebSocketV5PhotonApplication() : base(new ConfigurationManager()) {
        }

        protected override PeerBase CreatePeer(InitRequest initRequest) {
            return new ServerV5Peer(initRequest);
        }

        protected override void Setup() {
            GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(this.ApplicationRootPath, "log");
            GlobalContext.Properties["LogFileName"] = "ServerV5WebSocketInstanceLogs";

#if NETSTANDARD2_0 || NETCOREAPP
            var logRepository = log4net.LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.ConfigureAndWatch(logRepository, new FileInfo(Path.Combine(this.BinaryPath, "log4net.config")));
#else
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(this.BinaryPath, "log4net.config")));
#endif
        }

        protected override void TearDown() {
        }

    }
}