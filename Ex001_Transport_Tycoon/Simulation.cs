using System;
using System.Collections.Generic;
using System.Linq;

namespace Ex001_Transport_Tycoon
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

        public int Run(string initialCargo)
        {
            var cargos = CargosFromCharArray(initialCargo.ToCharArray());
            transports.Add(new Transport(Place.Factory, Place.Factory,TransportType.Vehicle));
            transports.Add(new Transport(Place.Port, Place.Port,TransportType.Ship));
            transports.Add(new Transport(Place.Factory, Place.Factory,TransportType.Vehicle));
            
            var totalTime = 0;
            
            map[Place.Factory].SetCargo(cargos);

            while (true)
            {
                foreach (var t in transports)
                {
                    if (t.IsEmpty() && t.CanLoadAt(map[t.CurrentLocation]))
                    {
                        t.LoadFrom(map[t.CurrentLocation]);
                        t.Move();
                    }
                    else if (t.IsLoaded() && t.ArrivedAtDestination())
                    {
                        var cargo = t.Unload();
                        map[t.CurrentLocation].AddCargo(cargo);
                        t.Move();
                    }
                    else
                    {
                        t.Move();
                    }
                }
                
                if (cargos.All(c => c.ArrivedAtDestination))
                    break;
                
                totalTime++;
            }

            return totalTime;
        }

        private List<Cargo> CargosFromCharArray(char[] initialCargo)
        {
            return initialCargo
                .Select(locationSymbol => 
                    new Cargo(Place.Factory, Enum.Parse<Place>(locationSymbol.ToString())))
                .ToList();
        }
        
    }
}