using System;
using System.Collections.Generic;

namespace MyMmo.Server {
    
    public class SubscriptionsCollection : IDisposable {

        private readonly IDisposable[] subscriptions;

        public SubscriptionsCollection(params IDisposable[] subscriptions) {
            this.subscriptions = subscriptions;
        }

        public void Dispose() {
            if (subscriptions == null) {
                return;
            }
            foreach (var subscription in subscriptions) {
                subscription?.Dispose();
            }
        }

    }
}