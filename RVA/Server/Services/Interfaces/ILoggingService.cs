using TravelSystem.Models.Enums;

namespace TravelSystem.Server.Services.Interfaces
{
    public interface ILoggingService
    {
        void Log(LogLevel level, string message);
        void LogError(string message, System.Exception exception = null);
        void LogInfo(string message);
        void LogWarning(string message);
        void LogDebug(string message);
    }
}
