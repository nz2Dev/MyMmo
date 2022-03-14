using System;
using System.Collections.Generic;

namespace MyMmo.Server {
    public class OnDisposeCallback : IDisposable {

        private readonly Action callback;

        public OnDisposeCallback(Action callback) {
            this.callback = callback;
        }

        public void Dispose() {
            callback();
        }

        public static OnDisposeCallback Create(Action callback) {
            return new OnDisposeCallback(callback);
        }

    }
    
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