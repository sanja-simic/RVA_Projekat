using System;
using TravelSystem.Models.Entities;
using TravelSystem.Models.Enums;
using TravelSystem.Server.DataAccess.Repositories;

namespace Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TravelSystem Server starting...");
            
            // Test new structure
            var repository = new TravelArrangementRepository();
            var destination = new Destination("1", "Paris", "France", 150.0);
            var arrangement = new TravelArrangement("arr1", ModeOfTransport.Plane, 7, destination);
            
            repository.Add(arrangement);
            var arrangements = repository.GetAll();
            
            Console.WriteLine($"Added arrangement: {arrangement.Id} to {arrangement.Destination.TownName}");
            Console.WriteLine($"Current state: {arrangement.GetCurrentStateName()}");
            
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
