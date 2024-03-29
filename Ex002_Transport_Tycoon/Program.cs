﻿using System;

namespace Ex002_Transport_Tycoon
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
                "AABABBAB"
            };

            
            foreach (var test in testData)
            {
                var s = new Simulation();
                var result = s.Run(test);
                Console.WriteLine($"Simulation {test} time  = {result.time}");

                Console.WriteLine("Events");
                foreach (var @event in result.events)
                {
                    Console.WriteLine(@event.ToJson());
                }

                LogFile.Create(test, result.time, result.events);

                Console.WriteLine("----------------------");
            }
        }
    }
}