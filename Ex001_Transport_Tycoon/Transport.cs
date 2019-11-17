namespace Ex001_Transport_Tycoon
{
    class Transport
    {
        public Place CurrentLocation { get; private set; }
        private Cargo cargo;
        private Place currentDestination;
        private int timeToDestination;
        private readonly TransportType type;
        private readonly Place baseLocation;

        public Transport(Place baseLocation, Place currentLocation, TransportType type)
        {
            this.CurrentLocation = currentLocation;
            this.type = type;
            this.baseLocation = baseLocation;
        }

        public void Move()
        {
            timeToDestination--;
            if (timeToDestination == 0)
            {
                CurrentLocation = currentDestination;
            }
        }

        public bool IsEmpty() => cargo == null;

        public void LoadFrom(Warehouse warehouse)
        {
            cargo = warehouse.GetCargoFor(this.type);
            (currentDestination,timeToDestination) = cargo.GetDestinationFor(this.type);
        }

        public bool CanLoadAt(Warehouse warehouse) => warehouse.HasCargoFor(type);

        public bool IsLoaded() => !IsEmpty();

        public bool ArrivedAtDestination() => currentDestination == CurrentLocation;

        public Cargo Unload()
        {
            var toUnload = cargo;
            toUnload.NextLeg();
            cargo = null;
            GoToBase();
            return toUnload;
        }

        private void GoToBase()
        {
            var wayHome = new RouteFinder().FindRoute(CurrentLocation, baseLocation, this.type);
            currentDestination = baseLocation;
            timeToDestination = wayHome.TotalTime;
        }
    }
    
    public enum TransportType
    {
        Vehicle, Ship        
    }
}