using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TravelSystem.Client.Views;
using TravelSystem.Client.Services;

namespace TravelSystem.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Process serverProcess;
        private readonly ILoggingService _loggingService;

        public App()
        {
            _loggingService = new LoggingService();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                _loggingService.Info("Client application starting");
                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                _loggingService.Fatal("Failed to start client application", ex);
                throw;
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                _loggingService.Info("Client application exiting");
                // Stop the server when the application exits
                StopServer();
                base.OnExit(e);
            }
            catch (Exception ex)
            {
                _loggingService.Error("Error during application exit", ex);
            }
        }

        public bool StartServerWithFormat(string format = "json")
        {
            return StartServer(format);
        }

        private bool StartServer(string format = "json")
        {
            try
            {
                _loggingService.Info($"Attempting to start server with format: {format}");
                
                // Multiple possible server paths
                string[] possiblePaths = new string[]
                {
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Server", "bin", "Debug", "Server.exe"),
                    Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, "Server", "bin", "Debug", "Server.exe"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Server", "bin", "Debug", "net472", "Server.exe"),
                    Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, "Server", "bin", "Debug", "net472", "Server.exe")
                };

                string serverPath = null;
                foreach (var path in possiblePaths)
                {
                    if (File.Exists(path))
                    {
                        serverPath = path;
                        _loggingService.Debug($"Server executable found at: {serverPath}");
                        break;
                    }
                }

                if (serverPath != null)
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = serverPath,
                        Arguments = format, // Use selected format
                        UseShellExecute = false,
                        CreateNoWindow = true, // Hide the console window
                        WindowStyle = ProcessWindowStyle.Hidden
                    };

                    serverProcess = Process.Start(startInfo);
                    _loggingService.Info($"Server process started successfully with PID: {serverProcess.Id}");
                    return true;
                }
                else
                {
                    _loggingService.Error("Server executable not found in any of the expected paths");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _loggingService.Error("Failed to start server", ex);
                MessageBox.Show($"Failed to start server: {ex.Message}", "Server Startup Error", 
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }

        public void StopServer()
        {
            try
            {
                if (serverProcess != null && !serverProcess.HasExited)
                {
                    _loggingService.Info($"Stopping server process with PID: {serverProcess.Id}");
                    serverProcess.Kill();
                    serverProcess.Dispose();
                    _loggingService.Info("Server process stopped successfully");
                }
                else
                {
                    _loggingService.Debug("Server process was already stopped or not running");
                }
            }
            catch (Exception ex)
            {
                _loggingService.Error("Error stopping server process", ex);
                // Log error but don't show to user during shutdown
                System.Diagnostics.Debug.WriteLine($"Error stopping server: {ex.Message}");
            }
        }
    }
}
