using System;
using System.Collections.Generic;
using System.Linq;

namespace Ex001_Transport_Tycoon
{
    class RouteFinder
    {
        private readonly List<RouteSpecification> routes = new List<RouteSpecification>
        {
            new RouteSpecification(new List<Leg>
            {
                new Leg(Place.Factory, Place.B, TransportType.Vehicle, 5)
            }),
            new RouteSpecification(new List<Leg>
            {
                new Leg(Place.B, Place.Factory, TransportType.Vehicle, 5)
            }),
            new RouteSpecification(new List<Leg>
            {
                new Leg(Place.Factory, Place.Port, TransportType.Vehicle, 1),
                new Leg(Place.Port, Place.A, TransportType.Ship, 4)
            }),
            new RouteSpecification(new List<Leg>
            {
                new Leg(Place.Port, Place.Factory, TransportType.Vehicle, 1)
            }),
            new RouteSpecification(new List<Leg>
            {
                new Leg(Place.A, Place.Port, TransportType.Ship, 4)
            })
        };

        public RouteSpecification FindRoute(Place from, Place to, TransportType transportType)
        {
            var route = routes.FirstOrDefault(r => r.Start == from && r.End == to && r.Legs.All(l=>l.RequiredTransportType==transportType));
            if (route == null)
            {
                throw new ApplicationException($"Route between {from} and {to} not found");
            }

            return route;
        }
        
        public RouteSpecification FindRoute(Place from, Place to)
        {
            var route = routes.FirstOrDefault(r => r.Start == from && r.End == to);
            if (route == null)
            {
                throw new ApplicationException("Route not found");
            }

            return route;
        }
    }
}