using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using TravelSystem.Client.Commands;
using TravelSystem.Client.Services;
using TravelSystem.Models.DTOs;
using TravelSystem.Models.Enums;
using TravelSystem.Contracts.DataContracts;

namespace TravelSystem.Client.ViewModels
{
    /// <summary>
    /// ViewModel for adding and editing travel arrangements with validation
    /// </summary>
    public class AddEditArrangementViewModel : BaseViewModel, IDataErrorInfo
    {
        private readonly TravelServiceClient _serviceClient;
        private readonly bool _isEditMode;

        // Properties for binding
        private string _id;
        private ModeOfTransport _selectedTransportation;
        private int _numberOfDays = 1;
        private DestinationDto _selectedDestination;
        private EntityState _selectedState;
        private double _totalPrice;
        private ObservableCollection<PassengerDto> _selectedPassengers;

        // Available options
        private ObservableCollection<DestinationDto> _availableDestinations;
        private ObservableCollection<PassengerDto> _availablePassengers;

        // Validation errors
        private readonly Dictionary<string, string> _errors = new Dictionary<string, string>();

        public AddEditArrangementViewModel(TravelArrangementDto arrangement = null)
        {
            _serviceClient = new TravelServiceClient();
            _isEditMode = arrangement != null;

            // Initialize collections
            _selectedPassengers = new ObservableCollection<PassengerDto>();
            _availableDestinations = new ObservableCollection<DestinationDto>();
            _availablePassengers = new ObservableCollection<PassengerDto>();

            // Initialize commands
                        SaveCommand = new RelayCommand(_ => Save(), _ => CanSave);
            CancelCommand = new RelayCommand(_ => Cancel());
            AddPassengerCommand = new RelayCommand(AddPassenger, _ => SelectedPassengerToAdd != null);
            RemovePassengerCommand = new RelayCommand(RemovePassenger, _ => SelectedPassengerInList != null);

            // Load data
            LoadAvailableData();

            // Initialize from existing arrangement if editing
            if (_isEditMode)
            {
                LoadArrangement(arrangement);
            }
            else
            {
                // Set defaults for new arrangement
                _id = Guid.NewGuid().ToString();
                _selectedTransportation = ModeOfTransport.Plane;
                _selectedState = EntityState.Reserved;
            }

            // Subscribe to property changes for automatic price calculation
            PropertyChanged += OnPropertyChanged;
        }

        #region Properties

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public ModeOfTransport SelectedTransportation
        {
            get => _selectedTransportation;
            set
            {
                if (SetProperty(ref _selectedTransportation, value))
                {
                    CalculateTotalPrice();
                }
            }
        }

        public int NumberOfDays
        {
            get => _numberOfDays;
            set
            {
                if (SetProperty(ref _numberOfDays, value))
                {
                    ValidateProperty(nameof(NumberOfDays));
                    CalculateTotalPrice();
                }
            }
        }

        public DestinationDto SelectedDestination
        {
            get => _selectedDestination;
            set
            {
                if (SetProperty(ref _selectedDestination, value))
                {
                    ValidateProperty(nameof(SelectedDestination));
                    CalculateTotalPrice();
                }
            }
        }

        public EntityState SelectedState
        {
            get => _selectedState;
            set => SetProperty(ref _selectedState, value);
        }

        public double TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }

        public ObservableCollection<PassengerDto> SelectedPassengers
        {
            get => _selectedPassengers;
            set => SetProperty(ref _selectedPassengers, value);
        }

        public ObservableCollection<DestinationDto> AvailableDestinations
        {
            get => _availableDestinations;
            set => SetProperty(ref _availableDestinations, value);
        }

        public ObservableCollection<PassengerDto> AvailablePassengers
        {
            get => _availablePassengers;
            set => SetProperty(ref _availablePassengers, value);
        }

        // Additional properties for passenger management
        public PassengerDto SelectedPassengerToAdd { get; set; }
        public PassengerDto SelectedPassengerInList { get; set; }

        // Available enum values
        public Array TransportationModes => Enum.GetValues(typeof(ModeOfTransport));
        public Array States => Enum.GetValues(typeof(EntityState));

        public bool IsEditMode => _isEditMode;
        public string WindowTitle => _isEditMode ? "Edit Travel Arrangement" : "Add New Travel Arrangement";

        public bool CanSave => !HasErrors && 
                              SelectedDestination != null && 
                              NumberOfDays > 0 && 
                              SelectedPassengers.Count > 0;

        public bool HasErrors => _errors.Any();

        #endregion

        #region Commands

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand AddPassengerCommand { get; }
        public ICommand RemovePassengerCommand { get; }

        #endregion

        #region IDataErrorInfo Implementation

        public string Error => string.Join(Environment.NewLine, _errors.Values);

        public string this[string columnName]
        {
            get
            {
                ValidateProperty(columnName);
                return _errors.ContainsKey(columnName) ? _errors[columnName] : null;
            }
        }

        #endregion

        #region Private Methods

        private void LoadAvailableData()
        {
            try
            {
                if (_serviceClient?.IsConnected == true)
                {
                    // Load destinations
                    var destinationsResponse = _serviceClient.Service.GetAllDestinations();
                    if (destinationsResponse.IsSuccess)
                    {
                        AvailableDestinations.Clear();
                        foreach (var destination in destinationsResponse.Data)
                        {
                            AvailableDestinations.Add(destination);
                        }
                    }

                    // Load passengers
                    var passengersResponse = _serviceClient.Service.GetAllPassengers();
                    if (passengersResponse.IsSuccess)
                    {
                        AvailablePassengers.Clear();
                        foreach (var passenger in passengersResponse.Data)
                        {
                            AvailablePassengers.Add(passenger);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle loading error
                System.Windows.MessageBox.Show($"Error loading data: {ex.Message}", "Error", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void LoadArrangement(TravelArrangementDto arrangement)
        {
            Id = arrangement.Id;
            SelectedTransportation = arrangement.AssociatedTransportation;
            NumberOfDays = arrangement.NumberOfDays;
            SelectedDestination = arrangement.Destination;
            SelectedState = arrangement.State;
            TotalPrice = arrangement.TotalPrice;

            SelectedPassengers.Clear();
            if (arrangement.Passengers != null)
            {
                foreach (var passenger in arrangement.Passengers)
                {
                    SelectedPassengers.Add(passenger);
                }
            }
        }

        private void ValidateProperty(string propertyName)
        {
            _errors.Remove(propertyName);

            switch (propertyName)
            {
                case nameof(NumberOfDays):
                    if (NumberOfDays <= 0)
                        _errors[propertyName] = "Number of days must be greater than 0";
                    else if (NumberOfDays > 365)
                        _errors[propertyName] = "Number of days cannot exceed 365";
                    break;

                case nameof(SelectedDestination):
                    if (SelectedDestination == null)
                        _errors[propertyName] = "Please select a destination";
                    break;
            }

            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(nameof(CanSave));
        }

        private void CalculateTotalPrice()
        {
            if (SelectedDestination != null && NumberOfDays > 0)
            {
                var basePrice = SelectedDestination.StayPriceByDay * NumberOfDays;
                
                // Add transportation cost based on mode
                double transportationMultiplier = 1.0;
                switch (SelectedTransportation)
                {
                    case ModeOfTransport.Plane:
                        transportationMultiplier = 2.0;
                        break;
                    case ModeOfTransport.Car:
                        transportationMultiplier = 1.2;
                        break;
                    case ModeOfTransport.Bus:
                        transportationMultiplier = 1.0;
                        break;
                    case ModeOfTransport.Ship:
                        transportationMultiplier = 1.5;
                        break;
                    default:
                        transportationMultiplier = 1.0;
                        break;
                }

                // Add cost per passenger
                var passengerCount = Math.Max(1, SelectedPassengers.Count);
                
                TotalPrice = basePrice * transportationMultiplier * passengerCount;
            }
            else
            {
                TotalPrice = 0;
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedPassengers))
            {
                CalculateTotalPrice();
            }
        }

        private void Save()
        {
            try
            {
                var arrangement = new TravelArrangementDto
                {
                    Id = Id,
                    AssociatedTransportation = SelectedTransportation,
                    NumberOfDays = NumberOfDays,
                    Destination = SelectedDestination,
                    State = SelectedState,
                    TotalPrice = TotalPrice,
                    Passengers = SelectedPassengers.ToList(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                ServiceResponse<TravelArrangementDto> response;
                
                if (_isEditMode)
                {
                    response = _serviceClient.Service.UpdateArrangement(arrangement);
                }
                else
                {
                    response = _serviceClient.Service.AddArrangement(arrangement);
                }

                if (response.IsSuccess)
                {
                    Result = arrangement;
                    OnSaveCompleted(true);
                }
                else
                {
                    System.Windows.MessageBox.Show($"Failed to save arrangement: {response.ErrorMessage}", 
                        "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error saving arrangement: {ex.Message}", 
                    "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void Cancel()
        {
            OnSaveCompleted(false);
        }

        private void AddPassenger(object parameter)
        {
            if (SelectedPassengerToAdd != null && !SelectedPassengers.Contains(SelectedPassengerToAdd))
            {
                SelectedPassengers.Add(SelectedPassengerToAdd);
                CalculateTotalPrice();
                OnPropertyChanged(nameof(CanSave));
            }
        }

        private void RemovePassenger(object parameter)
        {
            if (SelectedPassengerInList != null && SelectedPassengers.Contains(SelectedPassengerInList))
            {
                SelectedPassengers.Remove(SelectedPassengerInList);
                CalculateTotalPrice();
                OnPropertyChanged(nameof(CanSave));
            }
        }

        #endregion

        #region Events

        public event EventHandler<bool> SaveCompleted;

        private void OnSaveCompleted(bool saved)
        {
            SaveCompleted?.Invoke(this, saved);
        }

        #endregion

        #region Public Properties for Result

        public TravelArrangementDto Result { get; private set; }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _serviceClient?.Dispose();
        }

        #endregion
    }
}