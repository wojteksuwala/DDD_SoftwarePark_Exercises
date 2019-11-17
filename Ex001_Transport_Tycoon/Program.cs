using System;

namespace Ex001_Transport_Tycoon
{
    class Program
    {
        static void Main(string[] args)
        {
            var testData = new[]
            {
                "B",
                "A",
                "AB",
                "BB",
                "ABB", 
                "AA", 
                "AB",
                "BB",
                "ABB",
                "AABABBAB"
            };

            
            foreach (var test in testData)
            {
                var s = new Simulation();
                var time = s.Run(test);
                Console.WriteLine($"Simulation {test} time  = {time}");
            }
        }
    }

}
