using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;

namespace TravelSystem.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Process serverProcess;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Start the server in the background
            StartServer();
            
            // Give the server a moment to start
            Thread.Sleep(2000);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Stop the server when the application exits
            StopServer();
            base.OnExit(e);
        }

        private void StartServer()
        {
            try
            {
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
                        break;
                    }
                }

                if (serverPath != null)
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = serverPath,
                        UseShellExecute = false,
                        CreateNoWindow = true, // Hide the console window
                        WindowStyle = ProcessWindowStyle.Hidden
                    };

                    serverProcess = Process.Start(startInfo);
                }
                else
                {
                    MessageBox.Show("Server executable not found. Please build the Server project first.", 
                                   "Server Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start server: {ex.Message}", "Server Startup Error", 
                               MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void StopServer()
        {
            try
            {
                if (serverProcess != null && !serverProcess.HasExited)
                {
                    serverProcess.Kill();
                    serverProcess.Dispose();
                }
            }
            catch (Exception ex)
            {
                // Log error but don't show to user during shutdown
                System.Diagnostics.Debug.WriteLine($"Error stopping server: {ex.Message}");
            }
        }
    }
}
