namespace Ex002_Transport_Tycoon
{
    class Cargo
    {
        public int CargoId { get; }

        private readonly RouteSpecification routeSpecification;
        
        private Leg currentLeg;
        
        public Leg CurrentLeg => currentLeg;
        
        public bool ArrivedAtDestination { get; private set; }

        public Place Origin => routeSpecification.Start;

        public Place Destination => routeSpecification.End;

        public Cargo(int cargoId, Place current, Place destination)
        {
            CargoId = cargoId;
            routeSpecification = new RouteFinder().FindRoute(current, destination);
            currentLeg = routeSpecification.Legs[0];
            ArrivedAtDestination = false;
        }

        public bool CanBeTransportedBy(TransportType type) => currentLeg.RequiredTransportType == type;

        public void NextLeg()
        {
            var index = routeSpecification.Legs.IndexOf(currentLeg);
            if (index < routeSpecification.Legs.Count-1)
            {
                currentLeg = routeSpecification.Legs[index + 1];
            }
            else
            {
                ArrivedAtDestination = true;
            }
        }

        public (Place destination, int timeToArreive) GetDestinationFor(TransportType type)
        {
            var indexOfCurrentLeg = routeSpecification.Legs.IndexOf(CurrentLeg);
            var destinationForTransport = CurrentLeg.To;
            var timeToArrive = CurrentLeg.Time;

            for (var index = indexOfCurrentLeg+1; index <= routeSpecification.Legs.Count - 1; index++)
            {
                var leg = routeSpecification.Legs[index];
                
                if (leg.RequiredTransportType != type)
                    break;

                destinationForTransport = leg.To;
                timeToArrive += leg.Time;
            }
            
            return (destinationForTransport, timeToArrive);
        }
    }
}