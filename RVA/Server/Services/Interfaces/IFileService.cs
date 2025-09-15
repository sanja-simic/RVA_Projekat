using TravelSystem.Models.Enums;
using TravelSystem.Contracts.DataContracts;

namespace TravelSystem.Server.Services.Interfaces
{
    public interface IFileService
    {
        ServiceResponse<bool> SaveToFile(FileFormat format, string filePath);
        ServiceResponse<bool> LoadFromFile(FileFormat format, string filePath);
        ServiceResponse<string> ExportData(FileFormat format);
        ServiceResponse<bool> ImportData(FileFormat format, string data);
    }
}
