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
        
        public Cargo GetCargoFor(TransportType type)
        {
            var cargoToRemove = cargo.First(c => !c.ArrivedAtDestination && c.CanBeTransportedBy(type));
            cargo.Remove(cargoToRemove);
            return cargoToRemove;
        }

        public bool HasCargoFor(TransportType type)
        {
            return cargo.Any(c => !c.ArrivedAtDestination && c.CanBeTransportedBy(type));
        }

        public void AddCargo(Cargo newCargo) => cargo.Add(newCargo);
    }
}