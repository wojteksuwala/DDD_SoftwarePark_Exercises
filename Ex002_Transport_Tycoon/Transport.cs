using System.Collections.Generic;

namespace Ex002_Transport_Tycoon
{
    //TODO: load up to capacity
    //TODO: handle load time
    class Transport
    {
        public int TransportId { get; }
        public Place CurrentLocation { get; private set; }
        private List<Cargo> cargo = new List<Cargo>();
        
        private Place currentDestination;
        private int timeToDestination;
        private int eta;
        
        private int loadTime;
        private int etaLoad;
        private bool isLoading;
        private bool isUnloading;
        
        public TransportType Type { get; }
        private readonly Place baseLocation;
        public int Capacity { get; }
        private readonly List<DomainEvent> events = new List<DomainEvent>();

        public Transport(
            int transportId, 
            Place baseLocation, 
            Place currentLocation, 
            TransportType type,
            int capacity,
            int loadTime
            )
        {
            this.TransportId = transportId;
            this.CurrentLocation = currentLocation;
            this.Type = type;
            this.baseLocation = baseLocation;
            this.Capacity = capacity;
            this.loadTime = loadTime;
            this.currentDestination = baseLocation;
        }

        public void Move()
        {
            if (currentDestination==CurrentLocation)
                return;
            
            if (eta == timeToDestination)
            {
                events.Add(new TransportDeparts(this, CurrentLocation, currentDestination, cargo.ToArray()));
            }
            
            eta--;
            
            if (eta == 0)
            {
                CurrentLocation = currentDestination;
                events.Add(new TransportArrives(this, CurrentLocation, cargo.ToArray()));
            }
        }

        public bool IsEmpty() => cargo.Count==0;
        
        public bool IsLoading() => isLoading;

        public void Load(Warehouse warehouse)
        {
            if (!isLoading)
            {
                var cargoToLoad = warehouse.GetCargoFor(this);
                isLoading = true;
                etaLoad = loadTime;
                cargo.AddRange(cargoToLoad);
            }


            if (etaLoad == 0)
            {
                var (destination, timeDistance) = cargo[0].GetDestinationFor(this.Type);
                Start(destination, timeDistance);
                isLoading = false;
            }

            etaLoad--;
        }

        public bool CanLoadAt(Warehouse warehouse) => warehouse.HasCargoFor(Type);

        public bool IsLoaded() => !IsEmpty();

        public bool ArrivedAtDestination() => currentDestination == CurrentLocation;

        public List<Cargo> Unload()
        {
            if (!isUnloading)
            {
                isUnloading = true;
                etaLoad = loadTime;
            }

            if (etaLoad == 0)
            {
                var toUnload = new List<Cargo>(cargo);
                foreach (var c in toUnload)
                {
                    c.NextLeg();    
                }
                cargo.Clear();
                isUnloading = false;
                GoToBase();
                return toUnload;
            }

            etaLoad--;
            
            return new List<Cargo>{};
        }
        
        
        public IEnumerable<DomainEvent> GetEvents()
        {
            var eventsCopy = new List<DomainEvent>(events);
            events.Clear();
            return eventsCopy;
        }

        private void GoToBase()
        {
            if (CurrentLocation == baseLocation)
                return;
            var wayHome = new RouteFinder().FindRoute(CurrentLocation, baseLocation, this.Type);
            Start(baseLocation, wayHome.TotalTime);
        }

        private void Start(Place to,int distance)
        {
            currentDestination = to;
            timeToDestination = distance;
            eta = distance;
        }

    }
    
    public enum TransportType
    {
        Vehicle, Ship        
    }
}