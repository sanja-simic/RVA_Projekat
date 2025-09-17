using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using TravelSystem.Contracts.DataContracts;
using TravelSystem.Contracts.ServiceContracts;
using TravelSystem.Models.DTOs;
using TravelSystem.Models.Entities;
using TravelSystem.Models.Enums;
using TravelSystem.Server.DataAccess;
using TravelSystem.Server.DataAccess.FileHandlers;

namespace TravelSystem.Server.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public partial class TravelManagementService : ITravelManagementService
    {
        private static DataManager _dataManager;
        private static ILoggingService _loggingService;
        private readonly List<TravelArrangement> _arrangements;
        private readonly List<Destination> _destinations;
        private readonly List<Passenger> _passengers;

        public TravelManagementService()
        {
            _loggingService?.Info("TravelManagementService instance created");
            
            _arrangements = new List<TravelArrangement>();
            _destinations = new List<Destination>();
            _passengers = new List<Passenger>();
            
            // Load existing data or initialize with sample data
            InitializeData();
            
            _loggingService?.Info("TravelManagementService initialization completed");
        }

        public static void SetDataManager(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public static void SetLoggingService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        private void InitializeData()
        {
            try
            {
                _loggingService?.Info("Initializing data from storage");
                
                if (_dataManager != null)
                {
                    // Load existing data from files
                    var loadedArrangements = _dataManager.LoadData<TravelArrangement>("arrangements");
                    var loadedDestinations = _dataManager.LoadData<Destination>("destinations");
                    var loadedPassengers = _dataManager.LoadData<Passenger>("passengers");

                    _arrangements.AddRange(loadedArrangements);
                    _destinations.AddRange(loadedDestinations);
                    _passengers.AddRange(loadedPassengers);
                    
                    _loggingService?.Info($"Loaded data: {_arrangements.Count} arrangements, {_destinations.Count} destinations, {_passengers.Count} passengers");
                }

                // If no data loaded, create sample data
                if (!_destinations.Any())
                {
                    _loggingService?.Info("No existing data found, creating sample data");
                    CreateSampleData();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                _loggingService?.Error("Error loading data, creating sample data instead", ex);
                CreateSampleData();
            }
        }

        private void CreateSampleData()
        {
            // Add some sample destinations
            var paris = new Destination("1", "Paris", "France", 150.0);
            var london = new Destination("2", "London", "England", 200.0);
            var rome = new Destination("3", "Rome", "Italy", 120.0);
            
            _destinations.AddRange(new[] { paris, london, rome });

            // Add some sample passengers
            var passenger1 = new Passenger("1", "John", "Doe", "123456789", 23);
            var passenger2 = new Passenger("2", "Jane", "Smith", "987654321", 18);
            
            _passengers.AddRange(new[] { passenger1, passenger2 });

            // Add some sample arrangements
            var arrangement1 = new TravelArrangement("1", ModeOfTransport.Plane, 7, paris);
            arrangement1.Passengers.Add(passenger1);
            
            var arrangement2 = new TravelArrangement("2", ModeOfTransport.Train, 5, london);
            arrangement2.Passengers.Add(passenger2);
            
            _arrangements.AddRange(new[] { arrangement1, arrangement2 });

            // Save initial data
            SaveAllData();
        }

        private void SaveAllData()
        {
            if (_dataManager == null) return;

            try
            {
                _loggingService?.Debug("Saving all data to storage");
                
                _dataManager.SaveData(_arrangements, "arrangements");
                _dataManager.SaveData(_destinations, "destinations");
                _dataManager.SaveData(_passengers, "passengers");
                
                _loggingService?.Debug("All data saved successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
                _loggingService?.Error("Error saving data to storage", ex);
            }
        }

        public ServiceResponse<List<TravelArrangementDto>> GetAllArrangements()
        {
            try
            {
                _loggingService?.Debug("GetAllArrangements method called");
                
                var dtos = _arrangements.Select(ConvertToDto).ToList();
                
                _loggingService?.Info($"Retrieved {dtos.Count} arrangements");
                
                return new ServiceResponse<List<TravelArrangementDto>>
                {
                    Data = dtos,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _loggingService?.Error("Error retrieving arrangements", ex);
                
                return new ServiceResponse<List<TravelArrangementDto>>
                {
                    Data = null,
                    IsSuccess = false,
                    ErrorMessage = $"Error retrieving arrangements: {ex.Message}"
                };
            }
        }

        public ServiceResponse<TravelArrangementDto> GetArrangementById(string id)
        {
            try
            {
                var arrangement = _arrangements.FirstOrDefault(a => a.Id == id);
                
                if (arrangement == null)
                {
                    return new ServiceResponse<TravelArrangementDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ErrorMessage = "Arrangement not found"
                    };
                }

                return new ServiceResponse<TravelArrangementDto>
                {
                    Data = ConvertToDto(arrangement),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<TravelArrangementDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ErrorMessage = $"Error retrieving arrangement: {ex.Message}"
                };
            }
        }

        public ServiceResponse<TravelArrangementDto> AddArrangement(TravelArrangementDto arrangementDto)
        {
            try
            {
                _loggingService?.Info($"Adding new arrangement for destination: {arrangementDto.Destination?.TownName}");
                
                var arrangement = ConvertFromDto(arrangementDto);
                arrangement.Id = Guid.NewGuid().ToString();
                arrangement.CreatedAt = DateTime.Now;
                arrangement.UpdatedAt = DateTime.Now;
                
                _arrangements.Add(arrangement);
                SaveAllData();
                
                _loggingService?.Info($"Successfully added arrangement with ID: {arrangement.Id}");
                
                return new ServiceResponse<TravelArrangementDto>
                {
                    Data = ConvertToDto(arrangement),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _loggingService?.Error("Error adding arrangement", ex);
                
                return new ServiceResponse<TravelArrangementDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ErrorMessage = $"Error adding arrangement: {ex.Message}"
                };
            }
        }

        public ServiceResponse<TravelArrangementDto> UpdateArrangement(TravelArrangementDto arrangementDto)
        {
            try
            {
                var existingArrangement = _arrangements.FirstOrDefault(a => a.Id == arrangementDto.Id);
                if (existingArrangement == null)
                {
                    return new ServiceResponse<TravelArrangementDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ErrorMessage = "Arrangement not found"
                    };
                }

                var updatedArrangement = ConvertFromDto(arrangementDto);
                updatedArrangement.UpdatedAt = DateTime.Now;
                updatedArrangement.CreatedAt = existingArrangement.CreatedAt; // Preserve created date
                
                // Update the arrangement in the list
                var index = _arrangements.IndexOf(existingArrangement);
                _arrangements[index] = updatedArrangement;
                
                SaveAllData();
                
                return new ServiceResponse<TravelArrangementDto>
                {
                    Data = ConvertToDto(updatedArrangement),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<TravelArrangementDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ErrorMessage = $"Error updating arrangement: {ex.Message}"
                };
            }
        }

        public ServiceResponse<bool> DeleteArrangement(string id)
        {
            try
            {
                _loggingService?.Info($"Attempting to delete arrangement with ID: {id}");
                
                var arrangement = _arrangements.FirstOrDefault(a => a.Id == id);
                if (arrangement == null)
                {
                    _loggingService?.Warn($"Arrangement with ID {id} not found for deletion");
                    
                    return new ServiceResponse<bool>
                    {
                        Data = false,
                        IsSuccess = false,
                        ErrorMessage = "Arrangement not found"
                    };
                }

                _arrangements.Remove(arrangement);
                SaveAllData();
                
                _loggingService?.Info($"Successfully deleted arrangement with ID: {id}");
                
                return new ServiceResponse<bool>
                {
                    Data = true,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _loggingService?.Error($"Error deleting arrangement with ID: {id}", ex);
                
                return new ServiceResponse<bool>
                {
                    Data = false,
                    IsSuccess = false,
                    ErrorMessage = $"Error deleting arrangement: {ex.Message}"
                };
            }
        }

        public ServiceResponse<List<TravelArrangementDto>> SearchArrangements(SearchCriteriaDto criteria)
        {
            try
            {
                var arrangements = _arrangements.AsEnumerable();
                
                // Apply search filters
                if (!string.IsNullOrEmpty(criteria.Destination))
                {
                    arrangements = arrangements.Where(a => 
                        a.Destination.TownName.ToLowerInvariant().Contains(criteria.Destination.ToLowerInvariant()) ||
                        a.Destination.CountryName.ToLowerInvariant().Contains(criteria.Destination.ToLowerInvariant()));
                }
                
                if (criteria.MinPrice.HasValue)
                {
                    arrangements = arrangements.Where(a => GetTotalPrice(a) >= criteria.MinPrice.Value);
                }
                
                if (criteria.MaxPrice.HasValue)
                {
                    arrangements = arrangements.Where(a => GetTotalPrice(a) <= criteria.MaxPrice.Value);
                }
                
                var dtos = arrangements.Select(ConvertToDto).ToList();
                
                return new ServiceResponse<List<TravelArrangementDto>>
                {
                    Data = dtos,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<TravelArrangementDto>>
                {
                    Data = null,
                    IsSuccess = false,
                    ErrorMessage = $"Error searching arrangements: {ex.Message}"
                };
            }
        }

        public ServiceResponse<TravelArrangementDto> ChangeArrangementState(string id)
        {
            try
            {
                var arrangement = _arrangements.FirstOrDefault(a => a.Id == id);
                
                if (arrangement == null)
                {
                    return new ServiceResponse<TravelArrangementDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ErrorMessage = "Arrangement not found"
                    };
                }
                
                arrangement.ChangeState();
                SaveAllData();
                
                return new ServiceResponse<TravelArrangementDto>
                {
                    Data = ConvertToDto(arrangement),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<TravelArrangementDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ErrorMessage = $"Error changing arrangement state: {ex.Message}"
                };
            }
        }

        private TravelArrangementDto ConvertToDto(TravelArrangement arrangement)
        {
            return new TravelArrangementDto
            {
                Id = arrangement.Id,
                AssociatedTransportation = arrangement.AssociatedTransportation,
                NumberOfDays = arrangement.NumberOfDays,
                State = arrangement.State,
                CreatedAt = arrangement.CreatedAt,
                UpdatedAt = arrangement.UpdatedAt,
                Destination = new DestinationDto
                {
                    Id = arrangement.Destination.Id,
                    TownName = arrangement.Destination.TownName,
                    CountryName = arrangement.Destination.CountryName,
                    StayPriceByDay = arrangement.Destination.StayPriceByDay
                },
                Passengers = arrangement.Passengers.Select(p => new PassengerDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    PassportNumber = p.PassportNumber,
                    LuggageWeight = p.LuggageWeight
                }).ToList(),
                TotalPrice = GetTotalPrice(arrangement)
            };
        }

        private TravelArrangement ConvertFromDto(TravelArrangementDto dto)
        {
            var destination = new Destination(
                dto.Destination.Id,
                dto.Destination.TownName,
                dto.Destination.CountryName,
                dto.Destination.StayPriceByDay);

            var arrangement = new TravelArrangement(
                dto.Id,
                dto.AssociatedTransportation,
                dto.NumberOfDays,
                destination)
            {
                State = dto.State,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };

            if (dto.Passengers != null)
            {
                foreach (var passengerDto in dto.Passengers)
                {
                    var passenger = new Passenger(
                        passengerDto.Id,
                        passengerDto.FirstName,
                        passengerDto.LastName,
                        passengerDto.PassportNumber,
                        passengerDto.LuggageWeight);
                    
                    arrangement.Passengers.Add(passenger);
                }
            }

            return arrangement;
        }

        private double GetTotalPrice(TravelArrangement arrangement)
        {
            return arrangement.Destination.StayPriceByDay * arrangement.NumberOfDays;
        }

        // WCF Passenger Service Implementation
        public ServiceResponse<List<PassengerDto>> GetAllPassengers()
        {
            try
            {
                var passengerDtos = _passengers.Select(ConvertPassengerToDto).ToList();
                return new ServiceResponse<List<PassengerDto>>
                {
                    Data = passengerDtos,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<PassengerDto>>
                {
                    Data = null,
                    IsSuccess = false,
                    ErrorMessage = $"Error retrieving passengers: {ex.Message}"
                };
            }
        }

        public ServiceResponse<PassengerDto> GetPassengerById(string id)
        {
            try
            {
                var passenger = _passengers.FirstOrDefault(p => p.Id == id);
                if (passenger == null)
                {
                    return new ServiceResponse<PassengerDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ErrorMessage = "Passenger not found"
                    };
                }

                return new ServiceResponse<PassengerDto>
                {
                    Data = ConvertPassengerToDto(passenger),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<PassengerDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ErrorMessage = $"Error retrieving passenger: {ex.Message}"
                };
            }
        }

        public ServiceResponse<PassengerDto> AddPassenger(PassengerDto passengerDto)
        {
            try
            {
                // Check for duplicate passport number
                if (_passengers.Any(p => p.PassportNumber == passengerDto.PassportNumber))
                {
                    return new ServiceResponse<PassengerDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ErrorMessage = "A passenger with this passport number already exists"
                    };
                }

                var passenger = ConvertPassengerFromDto(passengerDto);
                passenger.Id = Guid.NewGuid().ToString();
                passenger.CreatedAt = DateTime.Now;
                passenger.UpdatedAt = DateTime.Now;

                if (!passenger.IsValid())
                {
                    return new ServiceResponse<PassengerDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ErrorMessage = passenger.GetValidationErrors()
                    };
                }

                _passengers.Add(passenger);

                return new ServiceResponse<PassengerDto>
                {
                    Data = ConvertPassengerToDto(passenger),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<PassengerDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ErrorMessage = $"Error adding passenger: {ex.Message}"
                };
            }
        }

        public ServiceResponse<PassengerDto> UpdatePassenger(PassengerDto passengerDto)
        {
            try
            {
                var existingPassenger = _passengers.FirstOrDefault(p => p.Id == passengerDto.Id);
                if (existingPassenger == null)
                {
                    return new ServiceResponse<PassengerDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ErrorMessage = "Passenger not found"
                    };
                }

                // Check for duplicate passport number (excluding current passenger)
                if (_passengers.Any(p => p.Id != passengerDto.Id && p.PassportNumber == passengerDto.PassportNumber))
                {
                    return new ServiceResponse<PassengerDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ErrorMessage = "A passenger with this passport number already exists"
                    };
                }

                var updatedPassenger = ConvertPassengerFromDto(passengerDto);
                updatedPassenger.CreatedAt = existingPassenger.CreatedAt;
                updatedPassenger.UpdatedAt = DateTime.Now;

                if (!updatedPassenger.IsValid())
                {
                    return new ServiceResponse<PassengerDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ErrorMessage = updatedPassenger.GetValidationErrors()
                    };
                }

                var index = _passengers.IndexOf(existingPassenger);
                _passengers[index] = updatedPassenger;

                return new ServiceResponse<PassengerDto>
                {
                    Data = ConvertPassengerToDto(updatedPassenger),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<PassengerDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ErrorMessage = $"Error updating passenger: {ex.Message}"
                };
            }
        }

        public ServiceResponse<bool> DeletePassenger(string id)
        {
            try
            {
                var passenger = _passengers.FirstOrDefault(p => p.Id == id);
                if (passenger == null)
                {
                    return new ServiceResponse<bool>
                    {
                        Data = false,
                        IsSuccess = false,
                        ErrorMessage = "Passenger not found"
                    };
                }

                // Check if passenger is being used in any arrangement
                var isPassengerInUse = _arrangements.Any(a => a.Passengers.Any(p => p.Id == id));
                
                if (isPassengerInUse)
                {
                    return new ServiceResponse<bool>
                    {
                        Data = false,
                        IsSuccess = false,
                        ErrorMessage = "Cannot delete passenger as they are assigned to one or more travel arrangements"
                    };
                }

                _passengers.Remove(passenger);
                SaveAllData();

                return new ServiceResponse<bool>
                {
                    Data = true,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    IsSuccess = false,
                    ErrorMessage = $"Error deleting passenger: {ex.Message}"
                };
            }
        }

        private PassengerDto ConvertPassengerToDto(Passenger passenger)
        {
            return new PassengerDto
            {
                Id = passenger.Id,
                FirstName = passenger.FirstName,
                LastName = passenger.LastName,
                PassportNumber = passenger.PassportNumber,
                LuggageWeight = passenger.LuggageWeight
            };
        }

        private Passenger ConvertPassengerFromDto(PassengerDto passengerDto)
        {
            return new Passenger(
                passengerDto.Id,
                passengerDto.FirstName,
                passengerDto.LastName,
                passengerDto.PassportNumber,
                passengerDto.LuggageWeight);
        }

        // WCF Destination Service Implementation
        public ServiceResponse<List<DestinationDto>> GetAllDestinations()
        {
            try
            {
                var destinationDtos = _destinations.Select(ConvertDestinationToDto).ToList();
                return new ServiceResponse<List<DestinationDto>>
                {
                    Data = destinationDtos,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<DestinationDto>>
                {
                    Data = null,
                    IsSuccess = false,
                    ErrorMessage = $"Error retrieving destinations: {ex.Message}"
                };
            }
        }

        public ServiceResponse<DestinationDto> GetDestinationById(string id)
        {
            try
            {
                var destination = _destinations.FirstOrDefault(d => d.Id == id);
                if (destination == null)
                {
                    return new ServiceResponse<DestinationDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ErrorMessage = "Destination not found"
                    };
                }

                return new ServiceResponse<DestinationDto>
                {
                    Data = ConvertDestinationToDto(destination),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<DestinationDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ErrorMessage = $"Error retrieving destination: {ex.Message}"
                };
            }
        }

        public ServiceResponse<DestinationDto> AddDestination(DestinationDto destinationDto)
        {
            try
            {
                var destination = ConvertDestinationFromDto(destinationDto);
                destination.Id = Guid.NewGuid().ToString();
                destination.CreatedAt = DateTime.Now;
                destination.UpdatedAt = DateTime.Now;

                if (!destination.IsValid())
                {
                    return new ServiceResponse<DestinationDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ErrorMessage = destination.GetValidationErrors()
                    };
                }

                _destinations.Add(destination);

                return new ServiceResponse<DestinationDto>
                {
                    Data = ConvertDestinationToDto(destination),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<DestinationDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ErrorMessage = $"Error adding destination: {ex.Message}"
                };
            }
        }

        public ServiceResponse<DestinationDto> UpdateDestination(DestinationDto destinationDto)
        {
            try
            {
                var existingDestination = _destinations.FirstOrDefault(d => d.Id == destinationDto.Id);
                if (existingDestination == null)
                {
                    return new ServiceResponse<DestinationDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ErrorMessage = "Destination not found"
                    };
                }

                var updatedDestination = ConvertDestinationFromDto(destinationDto);
                updatedDestination.CreatedAt = existingDestination.CreatedAt;
                updatedDestination.UpdatedAt = DateTime.Now;

                if (!updatedDestination.IsValid())
                {
                    return new ServiceResponse<DestinationDto>
                    {
                        Data = null,
                        IsSuccess = false,
                        ErrorMessage = updatedDestination.GetValidationErrors()
                    };
                }

                var index = _destinations.IndexOf(existingDestination);
                _destinations[index] = updatedDestination;

                return new ServiceResponse<DestinationDto>
                {
                    Data = ConvertDestinationToDto(updatedDestination),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<DestinationDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ErrorMessage = $"Error updating destination: {ex.Message}"
                };
            }
        }

        public ServiceResponse<bool> DeleteDestination(string id)
        {
            try
            {
                var destination = _destinations.FirstOrDefault(d => d.Id == id);
                if (destination == null)
                {
                    return new ServiceResponse<bool>
                    {
                        Data = false,
                        IsSuccess = false,
                        ErrorMessage = "Destination not found"
                    };
                }

                // Check if destination is being used in any arrangement
                var isDestinationInUse = _arrangements.Any(a => a.Destination.Id == id);
                
                if (isDestinationInUse)
                {
                    return new ServiceResponse<bool>
                    {
                        Data = false,
                        IsSuccess = false,
                        ErrorMessage = "Cannot delete destination as it is assigned to one or more travel arrangements"
                    };
                }

                _destinations.Remove(destination);
                SaveAllData();

                return new ServiceResponse<bool>
                {
                    Data = true,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    IsSuccess = false,
                    ErrorMessage = $"Error deleting destination: {ex.Message}"
                };
            }
        }

        private DestinationDto ConvertDestinationToDto(Destination destination)
        {
            return new DestinationDto
            {
                Id = destination.Id,
                TownName = destination.TownName,
                CountryName = destination.CountryName,
                StayPriceByDay = destination.StayPriceByDay
            };
        }

        private Destination ConvertDestinationFromDto(DestinationDto destinationDto)
        {
            return new Destination(
                destinationDto.Id,
                destinationDto.TownName,
                destinationDto.CountryName,
                destinationDto.StayPriceByDay);
        }
    }
}