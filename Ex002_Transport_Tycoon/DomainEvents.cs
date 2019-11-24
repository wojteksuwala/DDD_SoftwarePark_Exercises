using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Ex002_Transport_Tycoon
{
    abstract class DomainEvent
    {
        public string Event {get;}
        public int Time { get; }
        public int TransportId { get; }
        public TransportType Kind { get; }
        public Place? Location { get; }
        public Place? Destination { get; }
        public CargoInfo[] Cargo { get; }
        
        public int? Duration { get; }

        public DomainEvent(string @event, int time, int transportId, TransportType kind, Place? location, Place? destination, int? duration, Cargo[] cargo)
        {
            Event = @event;
            Time = time;
            TransportId = transportId;
            Kind = kind;
            Location = location;
            Destination = destination;
            Duration = duration;
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
            : base("DEPART", time, transportId, kind, location, destination, null, cargo)
        {
        }
    }

    class TransportArrived : DomainEvent
    {
        public TransportArrived(int time, int transportId, TransportType kind, Place destination, Cargo[] cargo) 
            : base("ARRIVE", time, transportId, kind, null, destination, null,cargo)
        {
        }
    }

    class CargoLoaded : DomainEvent
    {
        public CargoLoaded(int time, int transportId, TransportType kind, Place location, int duration, Cargo[] cargo) 
            : base("LOAD", time, transportId, kind, location, null, duration, cargo)
        {
        }
    }
    
    class CargoUnloaded : DomainEvent
    {
        public CargoUnloaded(int time, int transportId, TransportType kind, Place location, int duration, Cargo[] cargo) 
            : base("UNLOAD", time, transportId, kind, location, null, duration, cargo)
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