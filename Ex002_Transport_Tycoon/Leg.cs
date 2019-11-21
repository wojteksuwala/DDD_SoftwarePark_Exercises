namespace Ex002_Transport_Tycoon
{
    class Leg
    {
        public Place From { get; }
        public Place To { get; }
        public TransportType RequiredTransportType { get; }
        
        public int Time { get; }

        public Leg(Place @from, Place to, TransportType requiredTransportType, int time)
        {
            From = @from;
            To = to;
            RequiredTransportType = requiredTransportType;
            Time = time;
        }
    }
}