using System.Collections.Generic;
using System.Linq;

namespace Ex002_Transport_Tycoon
{
    class Transport
    {
        private readonly int transportId;
        private Place currentLocation;
        private readonly List<Cargo> cargo = new List<Cargo>();
        
        private Place currentDestination;
        private int timeToDestination;
        private int eta;
        
        private readonly int loadTime;
        private int etaLoad;
        private bool isLoading;
        private bool isUnloading;

        private readonly TransportType type;
        private readonly Place baseLocation;
        private readonly int capacity;
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
            this.transportId = transportId;
            this.currentLocation = currentLocation;
            this.type = type;
            this.baseLocation = baseLocation;
            this.capacity = capacity;
            this.loadTime = loadTime;
            this.currentDestination = baseLocation;
        }

        public IEnumerable<DomainEvent> Work(IDictionary<Place, Warehouse> map)
        {
            if ((IsEmpty() && CanLoadAt(map[currentLocation])) || isLoading)
            {
                Load(map[currentLocation]);
            }
            else if (IsLoaded() && ArrivedAtDestination())
            {
                var unloadedCargo = Unload();
                map[currentLocation].AddCargo(unloadedCargo);
            }
                    
            Move();
            
            return GetEvents();
        }

        private void Move()
        {
            if (currentDestination==currentLocation)
                return;
            
            if (eta == timeToDestination)
            {
                events.Add(new TransportDeparted(SimulationTime.Now(), transportId, type, currentLocation, currentDestination, cargo.ToArray()));
            }
            
            eta--;
            
            if (eta == 0)
            {
                currentLocation = currentDestination;
                events.Add(new TransportArrived(SimulationTime.Now(), transportId, type, currentLocation, cargo.ToArray()));
            }
        }

        private bool IsEmpty() => cargo.Count==0;

        private void Load(Warehouse warehouse)
        {
            if (!isLoading)
            {
                var cargoToLoad = warehouse.GetCargoFor(type,capacity);
                isLoading = true;
                etaLoad = loadTime;
                cargo.AddRange(cargoToLoad);
            }


            if (etaLoad == 0)
            {
                var (destination, timeDistance) = cargo[0].GetDestinationFor(type);
                Start(destination, timeDistance);
                isLoading = false;
                events.Add(new CargoLoaded(SimulationTime.Now(), transportId, type, currentLocation, cargo.ToArray()));
            }

            etaLoad--;
        }

        private bool CanLoadAt(Warehouse warehouse) => warehouse.HasCargoFor(type);

        private bool IsLoaded() => !IsEmpty();

        private bool ArrivedAtDestination() => currentDestination == currentLocation;

        private IList<Cargo> Unload()
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
                events.Add(new CargoUnloaded(SimulationTime.Now(), transportId, type, currentLocation, toUnload.ToArray()));
                return toUnload;
            }

            etaLoad--;
            
            return new List<Cargo>();
        }
        
        
        private IEnumerable<DomainEvent> GetEvents()
        {
            var eventsCopy = new List<DomainEvent>(events);
            events.Clear();
            return eventsCopy;
        }

        private void GoToBase()
        {
            if (currentLocation == baseLocation)
                return;
            var wayHome = new RouteFinder().FindRoute(currentLocation, baseLocation, type);
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