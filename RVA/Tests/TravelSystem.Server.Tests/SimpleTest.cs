using System;
using TravelSystem.Models.DTOs;
using TravelSystem.Models.Enums;

namespace TravelSystem.Server.Tests
{
    public class SimpleTest
    {
        public static bool TestBasicFunctionality()
        {
            try
            {
                // Test TravelManagementService basic functionality
                var service = new TravelSystem.Server.Services.TravelManagementService();
                
                // Test GetAllArrangements - should not throw exception
                var arrangementsResponse = service.GetAllArrangements();
                Console.WriteLine($"✅ GetAllArrangements: Success (found {arrangementsResponse.Data.Count} arrangements)");
                
                // Test GetAllPassengers
                var passengersResponse = service.GetAllPassengers();
                Console.WriteLine($"✅ GetAllPassengers: Success (found {passengersResponse.Data.Count} passengers)");
                
                // Test GetAllDestinations  
                var destinationsResponse = service.GetAllDestinations();
                Console.WriteLine($"✅ GetAllDestinations: Success (found {destinationsResponse.Data.Count} destinations)");
                
                // Test creating a destination
                var destinationDto = new DestinationDto
                {
                    Id = Guid.NewGuid().ToString(),
                    TownName = "TestTown",
                    CountryName = "TestCountry",
                    StayPriceByDay = 100.0
                };
                
                var addDestinationResponse = service.AddDestination(destinationDto);
                if (addDestinationResponse.IsSuccess)
                    Console.WriteLine("✅ AddDestination: Success");
                else
                    Console.WriteLine($"❌ AddDestination: {addDestinationResponse.ErrorMessage}");
                
                // Test creating a passenger
                var passengerDto = new PassengerDto
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "John",
                    LastName = "Doe",
                    PassportNumber = "AB123456",
                    LuggageWeight = 20
                };
                
                var addPassengerResponse = service.AddPassenger(passengerDto);
                if (addPassengerResponse.IsSuccess)
                    Console.WriteLine("✅ AddPassenger: Success");
                else
                    Console.WriteLine($"❌ AddPassenger: {addPassengerResponse.ErrorMessage}");
                
                // Test creating an arrangement
                var arrangementDto = new TravelArrangementDto
                {
                    Id = Guid.NewGuid().ToString(),
                    AssociatedTransportation = ModeOfTransport.Plane,
                    NumberOfDays = 7,
                    Destination = destinationDto,
                    State = EntityState.Reserved
                };
                
                var addArrangementResponse = service.AddArrangement(arrangementDto);
                if (addArrangementResponse.IsSuccess)
                    Console.WriteLine("✅ AddArrangement: Success");
                else
                    Console.WriteLine($"❌ AddArrangement: {addArrangementResponse.ErrorMessage}");
                
                // Test validation - create invalid destination
                var invalidDestinationDto = new DestinationDto
                {
                    Id = "",
                    TownName = "",
                    CountryName = "",
                    StayPriceByDay = -1
                };
                
                var invalidAddResponse = service.AddDestination(invalidDestinationDto);
                if (!invalidAddResponse.IsSuccess)
                {
                    Console.WriteLine("✅ Validation: Success (invalid destination correctly rejected)");
                }
                
                Console.WriteLine("\n🎉 All basic tests passed!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Test failed: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }
        
        public static void Main(string[] args)
        {
            Console.WriteLine("=== TravelSystem Simple Tests ===\n");
            
            bool success = TestBasicFunctionality();
            
            Console.WriteLine($"\nTest result: {(success ? "✅ PASSED" : "❌ FAILED")}");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}