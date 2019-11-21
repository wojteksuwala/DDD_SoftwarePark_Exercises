using System.Collections.Generic;

namespace Ex002_Transport_Tycoon
{
    class Transport
    {
        public int TransportId { get; }
        public Place CurrentLocation { get; private set; }
        private Cargo cargo;
        private Place currentDestination;
        private int timeToDestination;
        public TransportType Type { get; }
        private readonly Place baseLocation;
        private List<DomainEvent> events = new List<DomainEvent>();

        public Transport(int transportId, Place baseLocation, Place currentLocation, TransportType type)
        {
            this.TransportId = transportId;
            this.CurrentLocation = currentLocation;
            this.Type = type;
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
            cargo = warehouse.GetCargoFor(this.Type);
            (currentDestination,timeToDestination) = cargo.GetDestinationFor(this.Type);
            events.Add(new TransportDeparts(this, CurrentLocation, currentDestination, cargo!=null ? new Cargo[] {cargo} : new Cargo[]{}));
        }

        public bool CanLoadAt(Warehouse warehouse) => warehouse.HasCargoFor(Type);

        public bool IsLoaded() => !IsEmpty();

        public bool ArrivedAtDestination() => currentDestination == CurrentLocation;

        public Cargo Unload()
        {
            events.Add(new TransportArrives(this, CurrentLocation, cargo!=null ? new Cargo[] {cargo} : new Cargo[]{}));
            var toUnload = cargo;
            toUnload.NextLeg();
            cargo = null;
            GoToBase();
            return toUnload;
        }
        
        
        public IEnumerable<DomainEvent> GetEvents()
        {
            var eventsCopy = new List<DomainEvent>(events);
            events.Clear();
            return eventsCopy;
        }

        private void GoToBase()
        {
            var wayHome = new RouteFinder().FindRoute(CurrentLocation, baseLocation, this.Type);
            currentDestination = baseLocation;
            timeToDestination = wayHome.TotalTime;
        }

    }
    
    public enum TransportType
    {
        Vehicle, Ship        
    }
}