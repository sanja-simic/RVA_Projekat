using System.ServiceModel;
using TravelSystem.Contracts.DataContracts;
using TravelSystem.Models.Enums;

namespace TravelSystem.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface IFileOperationService
    {
        [OperationContract]
        ServiceResponse<bool> SaveToFile(FileFormat format, string filePath);

        [OperationContract]
        ServiceResponse<bool> LoadFromFile(FileFormat format, string filePath);

        [OperationContract]
        ServiceResponse<string> ExportData(FileFormat format);

        [OperationContract]
        ServiceResponse<bool> ImportData(FileFormat format, string data);
    }
}
