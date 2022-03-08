using ExitGames.Diagnostics.Counter;

namespace MyMmo.Server.Diagnostic {
    public class MessagesCounters {

        public static readonly CountsPerSecondCounter OtherMessagesReceiveCounter = new CountsPerSecondCounter("ItemMessage.Receive");
        
        public static readonly CountsPerSecondCounter OtherMessagesSendCounter = new CountsPerSecondCounter("ItemMessage.Send");

        public static readonly CountsPerSecondCounter ItemEventReceiveCounter = new CountsPerSecondCounter("ItemEventMessage.Received");
        
        public static readonly CountsPerSecondCounter ItemEventSendCounter = new CountsPerSecondCounter("ItemEventMessage.Send");
        
    }
}