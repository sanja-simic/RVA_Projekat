using System;
using System.ServiceModel;
using TravelSystem.Contracts.ServiceContracts;
using TravelSystem.Server.Services;

namespace Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TravelSystem Server starting...");
            
            // Create service host
            ServiceHost host = new ServiceHost(typeof(TravelSystem.Server.Services.TravelManagementService));
            
            try
            {
                // Configuration is now handled by App.config
                // No need to add endpoint manually
                
                // Open the service host
                host.Open();
                Console.WriteLine("TravelSystem Service is running at net.pipe://localhost/TravelService");
                Console.WriteLine("Press any key to stop the service...");
                
                // Keep the service running
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting service: {ex.Message}");
            }
            finally
            {
                // Close the service host
                if (host.State == CommunicationState.Opened)
                {
                    host.Close();
                }
            }
        }
    }
}
