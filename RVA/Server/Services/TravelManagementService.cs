using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using TravelSystem.Contracts.DataContracts;
using TravelSystem.Contracts.ServiceContracts;
using TravelSystem.Models.DTOs;
using TravelSystem.Models.Entities;
using TravelSystem.Models.Enums;
using TravelSystem.Server.DataAccess.Repositories;

namespace TravelSystem.Server.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class TravelManagementService : ITravelManagementService
    {
        private readonly TravelArrangementRepository _repository;
        private readonly List<Destination> _destinations;
        private readonly List<Passenger> _passengers;

        public TravelManagementService()
        {
            _repository = new TravelArrangementRepository();
            _destinations = new List<Destination>();
            _passengers = new List<Passenger>();
            
            // Initialize with some sample data
            InitializeSampleData();
        }

        private void InitializeSampleData()
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
            
            _repository.Add(arrangement1);
            _repository.Add(arrangement2);
        }

        public ServiceResponse<List<TravelArrangementDto>> GetAllArrangements()
        {
            try
            {
                var arrangements = _repository.GetAll();
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
                    ErrorMessage = $"Error retrieving arrangements: {ex.Message}"
                };
            }
        }

        public ServiceResponse<TravelArrangementDto> GetArrangementById(string id)
        {
            try
            {
                var arrangements = _repository.GetAll();
                var arrangement = arrangements.FirstOrDefault(a => a.Id == id);
                
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
                var arrangement = ConvertFromDto(arrangementDto);
                arrangement.Id = Guid.NewGuid().ToString();
                arrangement.CreatedAt = DateTime.Now;
                arrangement.UpdatedAt = DateTime.Now;
                
                _repository.Add(arrangement);
                
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
                    ErrorMessage = $"Error adding arrangement: {ex.Message}"
                };
            }
        }

        public ServiceResponse<TravelArrangementDto> UpdateArrangement(TravelArrangementDto arrangementDto)
        {
            try
            {
                var arrangement = ConvertFromDto(arrangementDto);
                arrangement.UpdatedAt = DateTime.Now;
                
                _repository.Update(arrangement);
                
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
                    ErrorMessage = $"Error updating arrangement: {ex.Message}"
                };
            }
        }

        public ServiceResponse<bool> DeleteArrangement(string id)
        {
            try
            {
                _repository.Delete(id);
                
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
                    ErrorMessage = $"Error deleting arrangement: {ex.Message}"
                };
            }
        }

        public ServiceResponse<List<TravelArrangementDto>> SearchArrangements(SearchCriteriaDto criteria)
        {
            try
            {
                var arrangements = _repository.GetAll();
                
                // Apply search filters
                if (!string.IsNullOrEmpty(criteria.Destination))
                {
                    arrangements = arrangements.Where(a => 
                        a.Destination.TownName.ToLowerInvariant().Contains(criteria.Destination.ToLowerInvariant()) ||
                        a.Destination.CountryName.ToLowerInvariant().Contains(criteria.Destination.ToLowerInvariant())).ToList();
                }
                
                if (criteria.MinPrice.HasValue)
                {
                    arrangements = arrangements.Where(a => GetTotalPrice(a) >= criteria.MinPrice.Value).ToList();
                }
                
                if (criteria.MaxPrice.HasValue)
                {
                    arrangements = arrangements.Where(a => GetTotalPrice(a) <= criteria.MaxPrice.Value).ToList();
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
                var arrangements = _repository.GetAll();
                var arrangement = arrangements.FirstOrDefault(a => a.Id == id);
                
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
                _repository.Update(arrangement);
                
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

        // Additional methods for managing destinations and passengers
        public List<Destination> GetAllDestinations()
        {
            return _destinations.ToList();
        }

        public List<Passenger> GetAllPassengers()
        {
            return _passengers.ToList();
        }

        public void AddDestination(Destination destination)
        {
            destination.Id = Guid.NewGuid().ToString();
            _destinations.Add(destination);
        }

        public void AddPassenger(Passenger passenger)
        {
            passenger.Id = Guid.NewGuid().ToString();
            _passengers.Add(passenger);
        }
    }
}