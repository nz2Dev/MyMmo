namespace MyMmo.Server {
    public static class Settings {

        static Settings() {
            MaxLockWaitTimeMilliseconds = 1000;
        }
        
        /// <summary>
        /// Maximum lock wait time for the lock protected dictionaries WorldCache.
        /// Default is 1 second.
        /// </summary>
        public static int MaxLockWaitTimeMilliseconds { get; set; }

    }
}