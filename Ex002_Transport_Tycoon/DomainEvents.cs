using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Ex002_Transport_Tycoon
{
    enum EventType
    {
        Depart,
        Arrive,
        Load,
        Unload
    }
    abstract class DomainEvent
    {
        public EventType Event {get;}
        public int Time { get; }
        public int TransportId { get; }
        public TransportType Kind { get; }
        public Place? Location { get; }
        public Place? Destination { get; }
        public CargoInfo[] Cargo { get; }

        public DomainEvent(EventType @event, int time, int transportId, TransportType kind, Place? location, Place? destination, Cargo[] cargo)
        {
            Event = @event;
            Time = time;
            TransportId = transportId;
            Kind = kind;
            Location = location;
            Destination = destination;
            Cargo = cargo
                .Select(c =>new CargoInfo(c))
                .ToArray();
        }

        public string ToJson()
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };
            var jsonSerializationSettings = new JsonSerializerSettings
            {
                Converters = new JsonConverter[] {new StringEnumConverter() },
                ContractResolver =  contractResolver,
                NullValueHandling = NullValueHandling.Ignore
            };
            return JsonConvert.SerializeObject(this,  jsonSerializationSettings);
        }
    }
    
    class TransportDeparted : DomainEvent
    {
        public TransportDeparted(int time, int transportId, TransportType kind, Place location, Place destination, Cargo[] cargo) 
            : base(EventType.Depart, time, transportId, kind, location, destination, cargo)
        {
        }
    }

    class TransportArrived : DomainEvent
    {
        public TransportArrived(int time, int transportId, TransportType kind, Place destination, Cargo[] cargo) 
            : base(EventType.Arrive, time, transportId, kind, null, destination, cargo)
        {
        }
    }

    class CargoLoaded : DomainEvent
    {
        public CargoLoaded(int time, int transportId, TransportType kind, Place location, Cargo[] cargo) 
            : base(EventType.Load, time, transportId, kind, location, null, cargo)
        {
        }
    }
    
    class CargoUnloaded : DomainEvent
    {
        public CargoUnloaded(int time, int transportId, TransportType kind, Place location, Cargo[] cargo) 
            : base(EventType.Unload, time, transportId, kind, location, null, cargo)
        {
        }
    }

    class CargoInfo
    {
        public int CargoId { get; }
        public Place Destination { get; }
        public Place Origin { get; }

        public CargoInfo(int cargoId, Place destination, Place origin)
        {
            CargoId = cargoId;
            Destination = destination;
            Origin = origin;
        }

        public CargoInfo(Cargo c)
        : this(c.CargoId, c.Destination, c.Origin)
        {
            
        }
    }

}