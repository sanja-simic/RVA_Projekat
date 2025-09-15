using System.Collections.Generic;
using System.ServiceModel;
using TravelSystem.Contracts.DataContracts;
using TravelSystem.Models.DTOs;
using TravelSystem.Models.Enums;

namespace TravelSystem.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface ITravelManagementService
    {
        [OperationContract]
        ServiceResponse<List<TravelArrangementDto>> GetAllArrangements();

        [OperationContract]
        ServiceResponse<TravelArrangementDto> GetArrangementById(string id);

        [OperationContract]
        ServiceResponse<TravelArrangementDto> AddArrangement(TravelArrangementDto arrangement);

        [OperationContract]
        ServiceResponse<TravelArrangementDto> UpdateArrangement(TravelArrangementDto arrangement);

        [OperationContract]
        ServiceResponse<bool> DeleteArrangement(string id);

        [OperationContract]
        ServiceResponse<List<TravelArrangementDto>> SearchArrangements(SearchCriteriaDto criteria);

        [OperationContract]
        ServiceResponse<TravelArrangementDto> ChangeArrangementState(string id);
    }
}
