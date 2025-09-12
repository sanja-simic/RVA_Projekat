using System;

namespace Tim11.Travel
{
    public class TestExample
    {
        public static void RunTest()
        {
            Console.WriteLine("=== Test Travel Management System ===\n");

            // Test osnovnih funkcionalnosti
            var destination = new Destination("1", "Pariz", "Francuska", 100.0);
            var arrangement = new TravelArrangement("TA001", ModeOfTransport.AIRPLANE, 5, destination);
            var passenger = new Passenger("P001", "Marko", "Petrović", "123456789", 20);
            arrangement.Passengers.Add(passenger);

            // Test Repository pattern sa različitim formatima
            Console.WriteLine("=== Test Repository Pattern ===");
            var csvRepository = RepositoryFactory.CreateRepository("CSV");
            var jsonRepository = RepositoryFactory.CreateRepository("JSON");
            var xmlRepository = RepositoryFactory.CreateRepository("XML");
            
            var travelService = new TravelService(jsonRepository);

            // Test Observer pattern
            Console.WriteLine("=== Test Observer Pattern ===");
            var emailObserver = new TravelNotificationObserver("Email Service");
            var smsObserver = new TravelNotificationObserver("SMS Service");
            
            travelService.RegisterObserver(emailObserver);
            travelService.RegisterObserver(smsObserver);

            // Test Command pattern
            Console.WriteLine("=== Test Command Pattern ===");
            var commandManager = new CommandManager();
            
            var addCommand = new AddTravelArrangementCommand(travelService, arrangement);
            commandManager.ExecuteCommand(addCommand);

            // Test State pattern
            Console.WriteLine("=== Test State Pattern ===");
            Console.WriteLine($"Početno stanje: {arrangement.GetCurrentStateName()}");
            arrangement.ChangeState(); // Reserved -> Paid
            Console.WriteLine($"Nakon promene: {arrangement.GetCurrentStateName()}");
            arrangement.ChangeState(); // Paid -> InProgress
            Console.WriteLine($"Nakon promene: {arrangement.GetCurrentStateName()}");
            arrangement.ChangeState(); // InProgress -> Completed
            Console.WriteLine($"Nakon promene: {arrangement.GetCurrentStateName()}");

            // Test Update Command
            arrangement.NumberOfDays = 7;
            var updateCommand = new UpdateTravelArrangementCommand(travelService, arrangement);
            commandManager.ExecuteCommand(updateCommand);

            // Test Undo/Redo
            Console.WriteLine("=== Test Undo/Redo ===");
            commandManager.UndoCommand();
            Console.WriteLine("Update command je poništen");
            
            commandManager.RedoCommand();
            Console.WriteLine("Update command je ponovo izvršen");

            // Test Adapter pattern
            Console.WriteLine("=== Test Adapter Pattern ===");
            var agencyArrangement = new AgencyArrangement(
                "AA001", 
                "Italija", 
                "AIRPLANE", 
                "Rim", 
                30,
                "Ana Marić",
                DateTime.Now,
                DateTime.Now.AddDays(7),
                1500.0
            );

            var adapter = new AgencyArrangementAdapter(agencyArrangement);
            var convertedArrangement = adapter.ConvertToTravelArrangement();
            Console.WriteLine($"Konvertovan aranžman: {convertedArrangement.Id} -> {convertedArrangement.Destination.TownName}");

            // Test Delete Command
            var deleteCommand = new DeleteTravelArrangementCommand(travelService, arrangement.Id);
            commandManager.ExecuteCommand(deleteCommand);

            Console.WriteLine("\n=== Test završen ===");
        }
    }
}
