using System;

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
}