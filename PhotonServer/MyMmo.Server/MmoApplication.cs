using System;
using System.IO;
using ExitGames.Logging.Log4Net;
using log4net;
using log4net.Config;
using Photon.SocketServer;
using LogManager = ExitGames.Logging.LogManager;

namespace MyMmo.Server {
    public class MmoApplication : ApplicationBase {
        
        protected override PeerBase CreatePeer(InitRequest initRequest) {
            return new MmoPeer(initRequest);
        }
        
        protected override void Setup() {
            GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(ApplicationRootPath, "log");
            var configFileInfo = new FileInfo(Path.Combine(BinaryPath, "log4net.config"));
            if (configFileInfo.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(configFileInfo);
            } else {
                throw new Exception("log4net Config file not found");
            }
        }

        protected override void TearDown() {
            
        }
    }
}