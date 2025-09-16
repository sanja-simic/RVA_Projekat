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

        // Passenger management operations
        [OperationContract]
        ServiceResponse<List<PassengerDto>> GetAllPassengers();

        [OperationContract]
        ServiceResponse<PassengerDto> GetPassengerById(string id);

        [OperationContract]
        ServiceResponse<PassengerDto> AddPassenger(PassengerDto passenger);

        [OperationContract]
        ServiceResponse<PassengerDto> UpdatePassenger(PassengerDto passenger);

        [OperationContract]
        ServiceResponse<bool> DeletePassenger(string id);

        // Destination management operations
        [OperationContract]
        ServiceResponse<List<DestinationDto>> GetAllDestinations();

        [OperationContract]
        ServiceResponse<DestinationDto> GetDestinationById(string id);

        [OperationContract]
        ServiceResponse<DestinationDto> AddDestination(DestinationDto destination);

        [OperationContract]
        ServiceResponse<DestinationDto> UpdateDestination(DestinationDto destination);

        [OperationContract]
        ServiceResponse<bool> DeleteDestination(string id);
    }
}
