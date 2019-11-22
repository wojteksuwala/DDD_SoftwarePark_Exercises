using System.Collections.Generic;
using System.Linq;

namespace Ex002_Transport_Tycoon
{
    class Warehouse
    {
        private readonly Place place;
        private readonly List<Cargo> cargo = new List<Cargo>();

        public Warehouse(Place place)
        {
            this.place = place;
        }

        public void SetCargo(IEnumerable<Cargo> initialCargo)
        {
            this.cargo.Clear();
            this.cargo.AddRange(initialCargo);
        }
        
        public List<Cargo> GetCargoFor(TransportType transportType, int capacity)
        {
            var cargoToRemove = cargo
                .Where(c => !c.ArrivedAtDestination && c.CanBeTransportedBy(transportType))
                .Take(capacity)
                .ToList();
            
            foreach (var c in cargoToRemove)
            {
                cargo.Remove(c);
            }
            return cargoToRemove;
        }

        public bool HasCargoFor(TransportType type)
        {
            return cargo.Any(c => !c.ArrivedAtDestination && c.CanBeTransportedBy(type));
        }

        public void AddCargo(IEnumerable<Cargo> newCargo) => cargo.AddRange(newCargo);
    }
}