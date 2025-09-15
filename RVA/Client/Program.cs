using System;
using TravelSystem.Models.Entities;
using TravelSystem.Models.Enums;
using TravelSystem.Models.DTOs;

namespace TravelSystem.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Travel System Client ===");
            Console.WriteLine("This is a console client demonstration.");
            Console.WriteLine("In production, this would be a WPF application with MVVM pattern.");
            Console.WriteLine();
            
            // Demo functionality
            Console.WriteLine("Creating sample travel arrangement...");
            
            var destination = new Destination("dest1", "Paris", "France", 150.0);
            var arrangement = new TravelArrangement("arr1", ModeOfTransport.Plane, 7, destination);
            
            var passenger = new Passenger("pass1", "Ana", "Petrovic", "AB123456", 20);
            arrangement.Passengers.Add(passenger);
            
            Console.WriteLine($"Arrangement: {arrangement.Id}");
            Console.WriteLine($"Destination: {arrangement.Destination.TownName}, {arrangement.Destination.CountryName}");
            Console.WriteLine($"Duration: {arrangement.NumberOfDays} days");
            Console.WriteLine($"Transport: {arrangement.AssociatedTransportation}");
            Console.WriteLine($"Current State: {arrangement.GetCurrentStateName()}");
            Console.WriteLine($"Passengers: {arrangement.Passengers.Count}");
            
            Console.WriteLine();
            Console.WriteLine("Changing state...");
            arrangement.ChangeState();
            Console.WriteLine($"New State: {arrangement.GetCurrentStateName()}");
            
            Console.WriteLine();
            Console.WriteLine("Validation:");
            Console.WriteLine($"Is Valid: {arrangement.IsValid()}");
            if (!arrangement.IsValid())
            {
                Console.WriteLine($"Errors: {arrangement.GetValidationErrors()}");
            }
            
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
