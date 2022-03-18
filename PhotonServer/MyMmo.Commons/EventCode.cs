namespace MyMmo.Commons {
    public enum EventCode : byte {
        
        ItemSubscribedEvent,

        ItemUnsubscribedEvent,
        
        ItemLocationChanged /*todo delete*/,

        ItemDestroyEvent,

        RegionUpdated

    }
}