using System;

namespace Ex002_Transport_Tycoon
{
    class SimulationTime
    {
        private static int now = 0;

        public static int Now() => now;

        public static void Elapse() => ++now;

        public static void Reset() => now = 0;
    }
}