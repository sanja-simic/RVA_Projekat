using System.Collections.Generic;
using TravelSystem.Models.DTOs;
using TravelSystem.Models.Entities;
using TravelSystem.Contracts.DataContracts;

namespace TravelSystem.Server.Services.Interfaces
{
    public interface ITravelManagementService
    {
        ServiceResponse<List<TravelArrangementDto>> GetAllArrangements();
        ServiceResponse<TravelArrangementDto> GetArrangementById(string id);
        ServiceResponse<TravelArrangementDto> AddArrangement(TravelArrangementDto arrangement);
        ServiceResponse<TravelArrangementDto> UpdateArrangement(TravelArrangementDto arrangement);
        ServiceResponse<bool> DeleteArrangement(string id);
        ServiceResponse<List<TravelArrangementDto>> SearchArrangements(SearchCriteriaDto criteria);
        ServiceResponse<TravelArrangementDto> ChangeArrangementState(string id);
    }
}
