using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Ex001_Transport_Tycoon
{
    class RouteSpecification
    {
        private readonly List<Leg> legs;

        public RouteSpecification(List<Leg> legs)
        {
            this.legs = legs;
        }

        public Place Start => legs[0].From;

        public Place End => legs.Last().To;

        public ReadOnlyCollection<Leg> Legs => legs.AsReadOnly();
        public int TotalTime => Legs.Sum(l => l.Time);
    }
}