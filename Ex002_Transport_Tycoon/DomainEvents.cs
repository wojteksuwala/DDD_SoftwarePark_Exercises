using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Ex002_Transport_Tycoon
{
    enum EventType
    {
        Depart,
        Arrive
    }
    abstract class DomainEvent
    {
        public EventType Event {get;}
        public int Time { get; }
        public int TransportId { get; }
        public TransportType Kind { get; }
        public Place? Location { get; }
        public Place Destination { get; }
        public CargoInfo[] Cargo { get; }

        public DomainEvent(EventType @event, int time, int transportId, TransportType kind, Place? location, Place destination, Cargo[] cargo)
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
    
    class TransportDeparts : DomainEvent
    {
        public TransportDeparts(int time, int transportId, TransportType kind, Place location, Place destination, Cargo[] cargo) 
            : base(EventType.Depart, time, transportId, kind, location, destination, cargo)
        {
        }
    }

    class TransportArrives : DomainEvent
    {
        public TransportArrives(int time, int transportId, TransportType kind, Place destinantion, Cargo[] cargo) 
            : base(EventType.Arrive, time, transportId, kind, null, destinantion, cargo)
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