using System;
using System.Collections.Generic;
using System.Linq;

namespace Ex002_Transport_Tycoon
{
    
    class Simulation
    {
        private readonly IDictionary<Place, Warehouse> map = new Dictionary<Place, Warehouse>()
        {
            [Place.Factory] = new Warehouse(Place.Factory),
            [Place.A] = new Warehouse(Place.A),
            [Place.B] = new Warehouse(Place.B),
            [Place.Port] = new Warehouse(Place.Port),
        };
        
        private readonly List<Transport> transports = new List<Transport>();

        public (int time,List<DomainEvent> events) Run(string initialCargo)
        {
            var cargos = CargosFromCharArray(initialCargo.ToCharArray());
            var events = new List<DomainEvent>();
            transports.Add(new Transport(0, Place.Factory, Place.Factory,TransportType.Vehicle,1,0));
            transports.Add(new Transport(1, Place.Factory, Place.Factory,TransportType.Vehicle,1,0));
            transports.Add(new Transport(2,Place.Port, Place.Port,TransportType.Ship,4,1));
            
            SimulationTime.Reset();
            
            map[Place.Factory].SetCargo(cargos);

            while (true)
            {
                foreach (var t in transports)
                {
                    events.AddRange(t.Work(map));
                }
                
                if (cargos.All(c => c.ArrivedAtDestination))
                    break;
                
                SimulationTime.Elapse();
            }

            return (SimulationTime.Now(), events);
        }

        private List<Cargo> CargosFromCharArray(char[] initialCargo)
        {
            var index = 0;
            return initialCargo
                .Select(locationSymbol => 
                    new Cargo(index++, Place.Factory, Enum.Parse<Place>(locationSymbol.ToString())))
                .ToList();
        }
        
    }
}