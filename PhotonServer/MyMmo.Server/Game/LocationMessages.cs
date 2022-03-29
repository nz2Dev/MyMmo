using Photon.SocketServer;

namespace MyMmo.Server.Game {
    
    public class LocationEventMessage {

        public EventData EventData { get; }
        public SendParameters SendParameters { get; }

        public LocationEventMessage(EventData eventData, SendParameters sendParameters) {
            EventData = eventData;
            SendParameters = sendParameters;
        }

    }
}