using System;
using System.ServiceModel;
using TravelSystem.Contracts.ServiceContracts;

namespace TravelSystem.Client.Services
{
    public class TravelServiceClient : IDisposable
    {
        private ChannelFactory<ITravelManagementService> channelFactory;
        private ITravelManagementService serviceProxy;
        private bool disposed = false;

        public TravelServiceClient()
        {
            ConnectToService();
        }

        private void ConnectToService()
        {
            try
            {
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress("http://localhost:8080/TravelService");
                
                channelFactory = new ChannelFactory<ITravelManagementService>(binding, endpoint);
                serviceProxy = channelFactory.CreateChannel();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to connect to travel service: {ex.Message}", ex);
            }
        }

        public ITravelManagementService Service
        {
            get
            {
                if (disposed)
                    throw new ObjectDisposedException(nameof(TravelServiceClient));
                    
                if (serviceProxy == null)
                    ConnectToService();
                    
                return serviceProxy;
            }
        }

        public bool IsConnected
        {
            get
            {
                try
                {
                    if (serviceProxy == null || channelFactory?.State != CommunicationState.Opened)
                        return false;

                    // Test the connection
                    var response = serviceProxy.GetAllArrangements();
                    return response != null;
                }
                catch
                {
                    return false;
                }
            }
        }

        public void Reconnect()
        {
            CloseConnection();
            ConnectToService();
        }

        private void CloseConnection()
        {
            try
            {
                if (channelFactory?.State == CommunicationState.Opened)
                {
                    channelFactory.Close();
                }
            }
            catch
            {
                channelFactory?.Abort();
            }
            finally
            {
                channelFactory = null;
                serviceProxy = null;
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                CloseConnection();
                disposed = true;
            }
        }
    }
}