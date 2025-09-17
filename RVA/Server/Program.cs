using System;
using System.ServiceModel;
using TravelSystem.Contracts.ServiceContracts;
using TravelSystem.Server.Services;
using TravelSystem.Server.DataAccess;
using TravelSystem.Server.DataAccess.FileHandlers;

namespace Server
{
    public class Program
    {
        private static ILoggingService _loggingService;
        
        static void Main(string[] args)
        {
            // Initialize logging first
            _loggingService = new LoggingService();
            _loggingService.Info("TravelSystem Server starting");
            
            Console.WriteLine("=== TravelSystem Server ===");
            Console.WriteLine();
            
            // Let user choose data storage format or use command line argument
            DataFileFormat selectedFormat;
            
            if (args.Length > 0 && TryParseFormatFromArgs(args[0], out selectedFormat))
            {
                Console.WriteLine($"Using format from command line: {selectedFormat}");
                _loggingService.Info($"Data format selected from command line: {selectedFormat}");
            }
            else
            {
                selectedFormat = ChooseDataFormat();
                _loggingService.Info($"Data format selected interactively: {selectedFormat}");
            }
            
            Console.WriteLine();
            Console.WriteLine($"Selected format: {selectedFormat}");
            Console.WriteLine("Initializing server...");
            _loggingService.Info("Initializing server components");
            
            // Create data manager with selected format
            var dataManager = new DataManager(selectedFormat);
            _loggingService.Debug($"DataManager created with format: {selectedFormat}");
            
            // Set data manager for the service
            TravelManagementService.SetDataManager(dataManager);
            TravelManagementService.SetLoggingService(_loggingService);
            _loggingService.Debug("TravelManagementService configured with DataManager and LoggingService");
            
            // Create service host
            ServiceHost host = new ServiceHost(typeof(TravelSystem.Server.Services.TravelManagementService));
            _loggingService.Debug("ServiceHost created");
            
            try
            {
                // Open the service host
                host.Open();
                
                Console.WriteLine();
                Console.WriteLine("✓ TravelSystem Service is running successfully!");
                Console.WriteLine("  Endpoint: net.pipe://localhost/TravelService");
                Console.WriteLine($"  Data format: {selectedFormat}");
                Console.WriteLine($"  Data directory: {dataManager.GetDataFilePath("").Replace("\\", "\\\\")}");
                Console.WriteLine();
                Console.WriteLine("Press any key to stop the service...");
                
                _loggingService.Info($"TravelSystem Service started successfully - Endpoint: net.pipe://localhost/TravelService, Format: {selectedFormat}");
                
                // Keep the service running
                Console.ReadKey();
                
                _loggingService.Info("Service stop requested by user input");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting service: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                _loggingService.Fatal("Error starting service", ex);
                Console.ReadKey();
            }
            finally
            {
                if (host.State == CommunicationState.Opened)
                {
                    host.Close();
                    _loggingService.Info("ServiceHost closed");
                }
                Console.WriteLine("Service stopped.");
                _loggingService.Info("TravelSystem Server stopped");
            }
        }
        
        private static bool TryParseFormatFromArgs(string arg, out DataFileFormat format)
        {
            format = DataFileFormat.JSON; // default
            
            switch (arg.ToLower())
            {
                case "xml":
                case "1":
                    format = DataFileFormat.XML;
                    return true;
                case "json":
                case "2":
                    format = DataFileFormat.JSON;
                    return true;
                case "csv":
                case "3":
                    format = DataFileFormat.CSV;
                    return true;
                default:
                    return false;
            }
        }
        
        private static DataFileFormat ChooseDataFormat()
        {
            while (true)
            {
                Console.WriteLine("Choose data storage format:");
                Console.WriteLine("1. XML");
                Console.WriteLine("2. JSON");
                Console.WriteLine("3. CSV");
                Console.Write("Enter your choice (1-3): ");
                
                var input = Console.ReadLine();
                
                switch (input)
                {
                    case "1":
                        return DataFileFormat.XML;
                    case "2":
                        return DataFileFormat.JSON;
                    case "3":
                        return DataFileFormat.CSV;
                    default:
                        Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                        Console.WriteLine();
                        break;
                }
            }
        }
    }
}
