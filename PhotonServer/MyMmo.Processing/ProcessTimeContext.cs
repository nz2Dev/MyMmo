namespace MyMmo.Processing {
    public struct ProcessTimeContext {

        public float StartTime;
        public float TimePassed;

        public float AbsoluteTimePassed() {
            return StartTime + TimePassed;
        }
        
    }
}