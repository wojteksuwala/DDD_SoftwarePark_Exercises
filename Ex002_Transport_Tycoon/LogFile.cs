using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ex002_Transport_Tycoon
{
    class LogFile
    {
        private readonly string scenario;
        private readonly int totalTime;
        private readonly List<DomainEvent> events;

        public LogFile(string scenario, int totalTime, List<DomainEvent> events)
        {
            this.scenario = scenario;
            this.totalTime = totalTime;
            this.events = events;
        }

        void Save()
        {
            var lines = new List<string> {$"# Deliver {scenario} completed in: {totalTime}"};
            lines.AddRange(events.Select(e=>e.ToJson()).ToList());
            File.WriteAllLines($"{scenario}.log", lines);
        }

        public static void Create(string scenario, int totalTime, List<DomainEvent> events)
        {
            var logFile = new LogFile(scenario,totalTime,events);
            logFile.Save();
        }
    }
}