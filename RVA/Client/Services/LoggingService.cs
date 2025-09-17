using System;
using System.IO;

namespace TravelSystem.Client.Services
{
    public interface ILoggingService
    {
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message, Exception exception = null);
        void Fatal(string message, Exception exception = null);
    }

    public class LoggingService : ILoggingService
    {
        private readonly string _logDirectory;
        private readonly string _logFile;
        private readonly string _errorLogFile;

        public LoggingService()
        {
            _logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "Client");
            Directory.CreateDirectory(_logDirectory);
            
            _logFile = Path.Combine(_logDirectory, "travel_client.log");
            _errorLogFile = Path.Combine(_logDirectory, "travel_client_error.log");
        }

        public void Debug(string message)
        {
            WriteLog("DEBUG", message);
        }

        public void Info(string message)
        {
            WriteLog("INFO", message);
        }

        public void Warn(string message)
        {
            WriteLog("WARN", message);
        }

        public void Error(string message, Exception exception = null)
        {
            var logMessage = exception != null ? $"{message} - Exception: {exception}" : message;
            WriteLog("ERROR", logMessage);
            WriteErrorLog("ERROR", logMessage);
        }

        public void Fatal(string message, Exception exception = null)
        {
            var logMessage = exception != null ? $"{message} - Exception: {exception}" : message;
            WriteLog("FATAL", logMessage);
            WriteErrorLog("FATAL", logMessage);
        }

        private void WriteLog(string level, string message)
        {
            try
            {
                var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] TravelSystem.Client - {message}{Environment.NewLine}";
                File.AppendAllText(_logFile, logEntry);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Logging error: {ex.Message}");
            }
        }

        private void WriteErrorLog(string level, string message)
        {
            try
            {
                var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] TravelSystem.Client - {message}{Environment.NewLine}";
                File.AppendAllText(_errorLogFile, logEntry);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error logging error: {ex.Message}");
            }
        }
    }
}