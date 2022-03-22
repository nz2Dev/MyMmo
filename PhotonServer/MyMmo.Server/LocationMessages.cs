using System;
using Photon.SocketServer;

namespace MyMmo.Server {
    public class RequestLocationSnapshotMessage {

        public RequestLocationSnapshotMessage(Action<LocationSnapshot> callback) {
            Callback = callback;
        }

        public Action<LocationSnapshot> Callback { get; }

    }
    
    public class LocationEventMessage {

        public EventData EventData { get; }
        public SendParameters SendParameters { get; }

        public LocationEventMessage(EventData eventData, SendParameters sendParameters) {
            EventData = eventData;
            SendParameters = sendParameters;
        }

    }
}