using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using TravelSystem.Client.Commands;
using TravelSystem.Client.Helpers;
using TravelSystem.Client.Services;
using TravelSystem.Client.Adapters;
using TravelSystem.Client.Legacy;
using TravelSystem.Models.DTOs;
using TravelSystem.Models.Enums;

namespace TravelSystem.Client.ViewModels
{
    /// <summary>
    /// Main ViewModel for the Travel Management System
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        private TravelServiceClient _serviceClient;
        private readonly UndoRedoManager _undoRedoManager;
        private readonly ILoggingService _loggingService;

        // Collections
        private ObservableCollection<TravelArrangementDto> _allArrangements;
        private ObservableCollection<TravelArrangementDto> _filteredArrangements;
        private ObservableCollection<PassengerDto> _passengers;
        private ObservableCollection<DestinationDto> _destinations;
        private ObservableCollection<StateTransition> _stateTransitions;

        // Selected items
        private TravelArrangementDto _selectedArrangement;
        private PassengerDto _selectedPassenger;
        private DestinationDto _selectedDestination;

        // Search and filtering
        private string _searchText;
        private EntityState? _selectedStateFilter;
        private ModeOfTransport? _selectedTransportationFilter;

        // Status
        private string _statusMessage;
        private bool _isConnected;

        public MainViewModel()
        {
            _loggingService = new LoggingService();
            _loggingService.Info("MainViewModel initializing");
            
            _undoRedoManager = new UndoRedoManager();
            _undoRedoManager.CanUndoRedoChanged += OnCanUndoRedoChanged;

            // Initialize collections
            _allArrangements = new ObservableCollection<TravelArrangementDto>();
            _filteredArrangements = new ObservableCollection<TravelArrangementDto>();
            _passengers = new ObservableCollection<PassengerDto>();
            _destinations = new ObservableCollection<DestinationDto>();
            _stateTransitions = new ObservableCollection<StateTransition>();

            // Initialize filtered view
            FilteredArrangementsView = CollectionViewSource.GetDefaultView(_filteredArrangements);
            FilteredArrangementsView.Filter = FilterArrangements;

            // Initialize commands
            InitializeCommands();

            // Initialize charts
            InitializeCharts();

            // Set initial connection status
            IsConnected = false;
            StatusMessage = "Click 'Connect' to start server and connect";
            
            _loggingService.Info("MainViewModel initialized successfully");
        }

        #region Properties

        public ObservableCollection<TravelArrangementDto> AllArrangements
        {
            get => _allArrangements;
            set => SetProperty(ref _allArrangements, value);
        }

        public ObservableCollection<TravelArrangementDto> FilteredArrangements
        {
            get => _filteredArrangements;
            set => SetProperty(ref _filteredArrangements, value);
        }

        public ICollectionView FilteredArrangementsView { get; }

        public ObservableCollection<PassengerDto> Passengers
        {
            get => _passengers;
            set => SetProperty(ref _passengers, value);
        }

        public ObservableCollection<DestinationDto> Destinations
        {
            get => _destinations;
            set => SetProperty(ref _destinations, value);
        }

        public ObservableCollection<StateTransition> StateTransitions
        {
            get => _stateTransitions;
            set => SetProperty(ref _stateTransitions, value);
        }

        public TravelArrangementDto SelectedArrangement
        {
            get => _selectedArrangement;
            set
            {
                if (SetProperty(ref _selectedArrangement, value))
                {
                    OnSelectedArrangementChanged();
                }
            }
        }

        public PassengerDto SelectedPassenger
        {
            get => _selectedPassenger;
            set => SetProperty(ref _selectedPassenger, value);
        }

        public DestinationDto SelectedDestination
        {
            get => _selectedDestination;
            set => SetProperty(ref _selectedDestination, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    ApplyFilters();
                }
            }
        }

        public EntityState? SelectedStateFilter
        {
            get => _selectedStateFilter;
            set
            {
                if (SetProperty(ref _selectedStateFilter, value))
                {
                    ApplyFilters();
                }
            }
        }

        public ModeOfTransport? SelectedTransportationFilter
        {
            get => _selectedTransportationFilter;
            set
            {
                if (SetProperty(ref _selectedTransportationFilter, value))
                {
                    ApplyFilters();
                }
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public bool IsConnected
        {
            get => _isConnected;
            set => SetProperty(ref _isConnected, value);
        }

        public bool CanUndo => _undoRedoManager.CanUndo;
        public bool CanRedo => _undoRedoManager.CanRedo;

        // Chart properties
        public ISeries[] StateSeries { get; set; }
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

        #endregion

        #region Commands

        public ICommand AddArrangementCommand { get; private set; }
        public ICommand EditArrangementCommand { get; private set; }
        public ICommand DeleteArrangementCommand { get; private set; }
        public ICommand RefreshArrangementsCommand { get; private set; }
        public ICommand ConnectToServerCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }
        public ICommand ClearFiltersCommand { get; private set; }
        public ICommand SimulateStateTransitionCommand { get; private set; }

        // Passenger commands
        public ICommand AddPassengerCommand { get; private set; }
        public ICommand EditPassengerCommand { get; private set; }
        public ICommand DeletePassengerCommand { get; private set; }
        public ICommand RefreshPassengersCommand { get; private set; }

        // Destination commands
        public ICommand AddDestinationCommand { get; private set; }
        public ICommand EditDestinationCommand { get; private set; }
        public ICommand DeleteDestinationCommand { get; private set; }
        public ICommand RefreshDestinationsCommand { get; private set; }
        public ICommand ImportLegacyDataCommand { get; private set; }

        #endregion

        #region Private Methods

        private void InitializeCommands()
        {
            // Arrangement commands
            AddArrangementCommand = new RelayCommand(_ => AddArrangement());
            EditArrangementCommand = new RelayCommand(_ => EditArrangement(), 
                _ => SelectedArrangement != null);
            DeleteArrangementCommand = new RelayCommand(_ => DeleteArrangement(), 
                _ => SelectedArrangement != null);
            RefreshArrangementsCommand = new RelayCommand(_ => RefreshArrangements());

            // Connection command
            ConnectToServerCommand = new RelayCommand(_ => InitializeServiceConnection());

            // Undo/Redo commands
            UndoCommand = new RelayCommand(_ => ExecuteUndo(), _ => _undoRedoManager.CanUndo);
            RedoCommand = new RelayCommand(_ => ExecuteRedo(), _ => _undoRedoManager.CanRedo);

            // Filter commands
            ClearFiltersCommand = new RelayCommand(_ => ClearFilters());

            // State simulation command
            SimulateStateTransitionCommand = new RelayCommand(_ => SimulateStateTransition(), 
                _ => SelectedArrangement != null);

            // Passenger commands
            AddPassengerCommand = new RelayCommand(_ => AddPassenger());
            EditPassengerCommand = new RelayCommand(_ => EditPassenger(), 
                _ => SelectedPassenger != null);
            DeletePassengerCommand = new RelayCommand(_ => DeletePassenger(), 
                _ => SelectedPassenger != null);
            RefreshPassengersCommand = new RelayCommand(_ => RefreshPassengers());

            // Destination commands
            AddDestinationCommand = new RelayCommand(_ => AddDestination());
            EditDestinationCommand = new RelayCommand(_ => EditDestination(), 
                _ => SelectedDestination != null);
            DeleteDestinationCommand = new RelayCommand(_ => DeleteDestination(), 
                _ => SelectedDestination != null);
            RefreshDestinationsCommand = new RelayCommand(_ => RefreshDestinations());

            // Legacy data integration command  
            ImportLegacyDataCommand = new RelayCommand(_ => ImportLegacyData());
        }

        private void InitializeCharts()
        {
            StateSeries = new ISeries[]
            {
                new PieSeries<double> { Name = "Reserved", Values = new double[] { 0 }, Fill = new SolidColorPaint(SKColors.Blue) },
                new PieSeries<double> { Name = "Paid", Values = new double[] { 0 }, Fill = new SolidColorPaint(SKColors.Green) },
                new PieSeries<double> { Name = "In Progress", Values = new double[] { 0 }, Fill = new SolidColorPaint(SKColors.Orange) },
                new PieSeries<double> { Name = "Completed", Values = new double[] { 0 }, Fill = new SolidColorPaint(SKColors.Gray) }
            };

            UpdateChartData();
        }

        private void InitializeServiceConnection()
        {
            try
            {
                _loggingService.Info("Attempting to initialize service connection");
                
                // If currently connected, disconnect
                if (IsConnected && _serviceClient?.IsConnected == true)
                {
                    _loggingService.Info("Currently connected, disconnecting first");
                    DisconnectFromServer();
                    return;
                }

                // Show format selection dialog
                var formatDialog = new Views.DataFormatSelectionWindow();
                var dialogResult = formatDialog.ShowDialog();
                
                if (dialogResult != true)
                {
                    _loggingService.Info("Connection cancelled by user");
                    StatusMessage = "Connection cancelled by user";
                    return;
                }

                _loggingService.Info($"Starting server with format: {formatDialog.SelectedFormat}");
                
                // Start server with selected format
                var app = (App)Application.Current;
                bool serverStarted = app.StartServerWithFormat(formatDialog.SelectedFormat);
                
                if (!serverStarted)
                {
                    _loggingService.Error("Failed to start server");
                    StatusMessage = "Failed to start server";
                    IsConnected = false;
                    return;
                }

                // Wait for server to start
                _loggingService.Debug("Waiting for server to start (3 seconds)");
                System.Threading.Thread.Sleep(3000);
                
                // Connect to server
                _serviceClient?.Dispose();
                _serviceClient = new TravelServiceClient();
                
                if (_serviceClient.IsConnected)
                {
                    IsConnected = true;
                    StatusMessage = $"Connected to server ({formatDialog.SelectedFormat.ToUpper()} format)";
                    _loggingService.Info($"Successfully connected to server with {formatDialog.SelectedFormat} format");
                    
                    RefreshArrangements();
                    RefreshPassengers();
                    RefreshDestinations();
                }
                else
                {
                    IsConnected = false;
                    StatusMessage = "Failed to connect to server";
                    _loggingService.Error("Failed to connect to server - service client connection failed");
                }
            }
            catch (Exception ex)
            {
                IsConnected = false;
                StatusMessage = $"Connection error: {ex.Message}";
                _loggingService.Error("Error during service connection initialization", ex);
            }
        }

        private void DisconnectFromServer()
        {
            try
            {
                _loggingService.Info("Disconnecting from server");
                
                _serviceClient?.Dispose();
                _serviceClient = null;
                
                var app = (App)Application.Current;
                app.StopServer();
                
                IsConnected = false;
                StatusMessage = "Disconnected from server";
                
                // Clear data
                AllArrangements.Clear();
                FilteredArrangements.Clear();
                Passengers.Clear();
                Destinations.Clear();
                
                _loggingService.Info("Successfully disconnected from server and cleared data");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Disconnect error: {ex.Message}";
                _loggingService.Error("Error during server disconnection", ex);
            }
        }

        private void AddArrangement()
        {
            try
            {
                var addEditWindow = new Views.AddEditArrangementWindow();
                if (addEditWindow.ShowDialog() == true)
                {
                    var newArrangement = addEditWindow.Arrangement;
                    if (newArrangement != null)
                    {
                        // Add to server
                        var response = _serviceClient.Service.AddArrangement(newArrangement);
                        
                        if (response.IsSuccess)
                        {
                            // Add to local collection using command pattern for undo/redo
                            var addCommand = new AddTravelArrangementCommand(AllArrangements, response.Data);
                            _loggingService.Info($"Adding arrangement command to undo stack. Current CanUndo: {_undoRedoManager.CanUndo}");
                            _undoRedoManager.ExecuteCommand(addCommand);
                            _loggingService.Info($"After adding command. CanUndo: {_undoRedoManager.CanUndo}, CanRedo: {_undoRedoManager.CanRedo}");
                            
                            ApplyFilters();
                            UpdateChartData();
                            StatusMessage = "Arrangement added successfully";
                            
                            // Select the newly added arrangement
                            SelectedArrangement = response.Data;
                        }
                        else
                        {
                            MessageBox.Show($"Failed to add arrangement: {response.ErrorMessage}", 
                                          "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding arrangement: {ex.Message}", 
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditArrangement()
        {
            if (SelectedArrangement == null) return;

            try
            {
                var addEditWindow = new Views.AddEditArrangementWindow(SelectedArrangement);
                if (addEditWindow.ShowDialog() == true)
                {
                    var updatedArrangement = addEditWindow.Arrangement;
                    if (updatedArrangement != null)
                    {
                        // Update on server
                        var response = _serviceClient.Service.UpdateArrangement(updatedArrangement);
                        
                        if (response.IsSuccess)
                        {
                            // Create command for undo/redo
                            var originalArrangement = new TravelArrangementDto
                            {
                                Id = SelectedArrangement.Id,
                                AssociatedTransportation = SelectedArrangement.AssociatedTransportation,
                                NumberOfDays = SelectedArrangement.NumberOfDays,
                                Destination = SelectedArrangement.Destination,
                                State = SelectedArrangement.State,
                                TotalPrice = SelectedArrangement.TotalPrice,
                                Passengers = SelectedArrangement.Passengers?.ToList() ?? new List<PassengerDto>(),
                                CreatedAt = SelectedArrangement.CreatedAt,
                                UpdatedAt = SelectedArrangement.UpdatedAt
                            };
                            var updateCommand = new UpdateTravelArrangementCommand(
                                AllArrangements, originalArrangement, response.Data);
                            _undoRedoManager.ExecuteCommand(updateCommand);
                            
                            ApplyFilters();
                            UpdateChartData();
                            StatusMessage = "Arrangement updated successfully";
                            
                            // Update selection
                            SelectedArrangement = response.Data;
                        }
                        else
                        {
                            MessageBox.Show($"Failed to update arrangement: {response.ErrorMessage}", 
                                          "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing arrangement: {ex.Message}", 
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteArrangement()
        {
            if (SelectedArrangement == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete the arrangement to {SelectedArrangement.Destination?.TownName}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var response = _serviceClient.Service.DeleteArrangement(SelectedArrangement.Id);
                    
                    if (response.IsSuccess)
                    {
                        var deleteCommand = new DeleteTravelArrangementCommand(AllArrangements, SelectedArrangement);
                        _undoRedoManager.ExecuteCommand(deleteCommand);
                        
                        ApplyFilters();
                        UpdateChartData();
                        StatusMessage = "Arrangement deleted successfully";
                    }
                    else
                    {
                        MessageBox.Show($"Failed to delete arrangement: {response.ErrorMessage}", 
                                      "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting arrangement: {ex.Message}", 
                                  "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RefreshArrangements()
        {
            try
            {
                if (_serviceClient?.IsConnected == true)
                {
                    var response = _serviceClient.Service.GetAllArrangements();
                    
                    if (response.IsSuccess)
                    {
                        AllArrangements.Clear();
                        foreach (var arrangement in response.Data)
                        {
                            AllArrangements.Add(arrangement);
                        }
                        
                        ApplyFilters();
                        UpdateChartData();
                        StatusMessage = $"Loaded {AllArrangements.Count} arrangements";
                    }
                    else
                    {
                        StatusMessage = $"Failed to load arrangements: {response.ErrorMessage}";
                    }
                }
                else
                {
                    StatusMessage = "Not connected to server";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading arrangements: {ex.Message}";
            }
        }

        private void ApplyFilters()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                FilteredArrangements.Clear();
                
                var filtered = AllArrangements.AsEnumerable();
                
                // Apply search filter
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    filtered = SearchHelper.SearchArrangements(filtered, SearchText);
                }
                
                // Apply state filter
                if (SelectedStateFilter.HasValue)
                {
                    filtered = SearchHelper.FilterByState(filtered, SelectedStateFilter);
                }
                
                // Apply transportation filter
                if (SelectedTransportationFilter.HasValue)
                {
                    filtered = SearchHelper.FilterByTransportation(filtered, SelectedTransportationFilter);
                }
                
                foreach (var arrangement in filtered)
                {
                    FilteredArrangements.Add(arrangement);
                }
                
                FilteredArrangementsView.Refresh();
            });
        }

        private bool FilterArrangements(object item)
        {
            // Additional filtering logic can be added here if needed
            return true;
        }

        private void ClearFilters()
        {
            SearchText = string.Empty;
            SelectedStateFilter = null;
            SelectedTransportationFilter = null;
        }

        private void UpdateChartData()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var stateCounts = AllArrangements
                    .GroupBy(a => a.State)
                    .ToDictionary(g => g.Key, g => g.Count());

                var reservedCount = stateCounts.ContainsKey(EntityState.Reserved) ? stateCounts[EntityState.Reserved] : 0;
                var paidCount = stateCounts.ContainsKey(EntityState.Paid) ? stateCounts[EntityState.Paid] : 0;
                var inProgressCount = stateCounts.ContainsKey(EntityState.InProgress) ? stateCounts[EntityState.InProgress] : 0;
                var completedCount = stateCounts.ContainsKey(EntityState.Completed) ? stateCounts[EntityState.Completed] : 0;

                StateSeries[0].Values = new double[] { reservedCount };
                StateSeries[1].Values = new double[] { paidCount };
                StateSeries[2].Values = new double[] { inProgressCount };
                StateSeries[3].Values = new double[] { completedCount };

                OnPropertyChanged(nameof(StateSeries));
            });
        }

        private void SimulateStateTransition()
        {
            if (SelectedArrangement == null) return;

            try
            {
                var transitions = StateTransitionHelper.GetAllPossibleTransitions(SelectedArrangement);
                
                StateTransitions.Clear();
                foreach (var transition in transitions)
                {
                    StateTransitions.Add(transition);
                }

                // Simulate automatic progression to next state
                var nextState = StateTransitionHelper.SimulateNextState(SelectedArrangement.State);
                if (nextState != SelectedArrangement.State)
                {
                    SelectedArrangement.State = nextState;
                    OnPropertyChanged(nameof(SelectedArrangement));
                    
                    // Update the arrangement on server
                    var response = _serviceClient.Service.UpdateArrangement(SelectedArrangement);
                    if (response.IsSuccess)
                    {
                        UpdateChartData();
                        StatusMessage = $"State changed to {nextState}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error simulating state transition: {ex.Message}", 
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnSelectedArrangementChanged()
        {
            if (SelectedArrangement != null)
            {
                var transitions = StateTransitionHelper.GetAllPossibleTransitions(SelectedArrangement);
                StateTransitions.Clear();
                foreach (var transition in transitions)
                {
                    StateTransitions.Add(transition);
                }
            }
        }

        private void OnCanUndoRedoChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(CanUndo));
            OnPropertyChanged(nameof(CanRedo));
            
            // Force command manager to update button states
            System.Windows.Input.CommandManager.InvalidateRequerySuggested();
        }

        private void ExecuteUndo()
        {
            try
            {
                _loggingService.Info($"ExecuteUndo called. CanUndo: {_undoRedoManager.CanUndo}");
                
                if (_undoRedoManager.CanUndo)
                {
                    _undoRedoManager.Undo();
                    ApplyFilters();
                    UpdateChartData();
                    StatusMessage = "Undo executed successfully";
                    _loggingService.Info("Undo executed successfully");
                }
                else
                {
                    _loggingService.Warn("Undo called but CanUndo is false");
                }
            }
            catch (Exception ex)
            {
                _loggingService.Error("Error executing undo", ex);
                StatusMessage = $"Undo error: {ex.Message}";
            }
        }

        private void ExecuteRedo()
        {
            try
            {
                _loggingService.Info($"ExecuteRedo called. CanRedo: {_undoRedoManager.CanRedo}");
                
                if (_undoRedoManager.CanRedo)
                {
                    _undoRedoManager.Redo();
                    ApplyFilters();
                    UpdateChartData();
                    StatusMessage = "Redo executed successfully";
                    _loggingService.Info("Redo executed successfully");
                }
                else
                {
                    _loggingService.Warn("Redo called but CanRedo is false");
                }
            }
            catch (Exception ex)
            {
                _loggingService.Error("Error executing redo", ex);
                StatusMessage = $"Redo error: {ex.Message}";
            }
        }

        // Passenger operations
        private void AddPassenger()
        {
            try
            {
                var addEditWindow = new Views.AddEditPassengerWindow();
                if (addEditWindow.ShowDialog() == true)
                {
                    var newPassenger = addEditWindow.Passenger;
                    if (newPassenger != null)
                    {
                        var response = _serviceClient.Service.AddPassenger(newPassenger);
                        
                        if (response.IsSuccess)
                        {
                            Passengers.Add(response.Data);
                            StatusMessage = "Passenger added successfully";
                        }
                        else
                        {
                            MessageBox.Show($"Failed to add passenger: {response.ErrorMessage}", 
                                          "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding passenger: {ex.Message}", 
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditPassenger()
        {
            if (SelectedPassenger == null) return;

            try
            {
                var addEditWindow = new Views.AddEditPassengerWindow(SelectedPassenger);
                if (addEditWindow.ShowDialog() == true)
                {
                    var updatedPassenger = addEditWindow.Passenger;
                    if (updatedPassenger != null)
                    {
                        var response = _serviceClient.Service.UpdatePassenger(updatedPassenger);
                        
                        if (response.IsSuccess)
                        {
                            var index = Passengers.IndexOf(SelectedPassenger);
                            if (index >= 0)
                            {
                                Passengers[index] = response.Data;
                            }
                            StatusMessage = "Passenger updated successfully";
                        }
                        else
                        {
                            MessageBox.Show($"Failed to update passenger: {response.ErrorMessage}", 
                                          "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing passenger: {ex.Message}", 
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeletePassenger()
        {
            if (SelectedPassenger == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete passenger {SelectedPassenger.FirstName} {SelectedPassenger.LastName}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var response = _serviceClient.Service.DeletePassenger(SelectedPassenger.Id);
                    
                    if (response.IsSuccess)
                    {
                        Passengers.Remove(SelectedPassenger);
                        StatusMessage = "Passenger deleted successfully";
                    }
                    else
                    {
                        MessageBox.Show($"Failed to delete passenger: {response.ErrorMessage}", 
                                      "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting passenger: {ex.Message}", 
                                  "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RefreshPassengers()
        {
            try
            {
                if (_serviceClient?.IsConnected == true)
                {
                    var response = _serviceClient.Service.GetAllPassengers();
                    
                    if (response.IsSuccess)
                    {
                        Passengers.Clear();
                        foreach (var passenger in response.Data)
                        {
                            Passengers.Add(passenger);
                        }
                        StatusMessage = $"Loaded {Passengers.Count} passengers";
                    }
                    else
                    {
                        StatusMessage = $"Failed to load passengers: {response.ErrorMessage}";
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading passengers: {ex.Message}";
            }
        }

        // Destination operations
        private void AddDestination()
        {
            try
            {
                var addEditWindow = new Views.AddEditDestinationWindow();
                if (addEditWindow.ShowDialog() == true)
                {
                    var newDestination = addEditWindow.Destination;
                    if (newDestination != null)
                    {
                        var response = _serviceClient.Service.AddDestination(newDestination);
                        
                        if (response.IsSuccess)
                        {
                            Destinations.Add(response.Data);
                            StatusMessage = "Destination added successfully";
                        }
                        else
                        {
                            MessageBox.Show($"Failed to add destination: {response.ErrorMessage}", 
                                          "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding destination: {ex.Message}", 
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditDestination()
        {
            if (SelectedDestination == null) return;

            try
            {
                var addEditWindow = new Views.AddEditDestinationWindow(SelectedDestination);
                if (addEditWindow.ShowDialog() == true)
                {
                    var updatedDestination = addEditWindow.Destination;
                    if (updatedDestination != null)
                    {
                        var response = _serviceClient.Service.UpdateDestination(updatedDestination);
                        
                        if (response.IsSuccess)
                        {
                            var index = Destinations.IndexOf(SelectedDestination);
                            if (index >= 0)
                            {
                                Destinations[index] = response.Data;
                            }
                            StatusMessage = "Destination updated successfully";
                        }
                        else
                        {
                            MessageBox.Show($"Failed to update destination: {response.ErrorMessage}", 
                                          "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing destination: {ex.Message}", 
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteDestination()
        {
            if (SelectedDestination == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete destination {SelectedDestination.TownName}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var response = _serviceClient.Service.DeleteDestination(SelectedDestination.Id);
                    
                    if (response.IsSuccess)
                    {
                        Destinations.Remove(SelectedDestination);
                        StatusMessage = "Destination deleted successfully";
                    }
                    else
                    {
                        MessageBox.Show($"Failed to delete destination: {response.ErrorMessage}", 
                                      "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting destination: {ex.Message}", 
                                  "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RefreshDestinations()
        {
            try
            {
                if (_serviceClient?.IsConnected == true)
                {
                    var response = _serviceClient.Service.GetAllDestinations();
                    
                    if (response.IsSuccess)
                    {
                        Destinations.Clear();
                        foreach (var destination in response.Data)
                        {
                            Destinations.Add(destination);
                        }
                        StatusMessage = $"Loaded {Destinations.Count} destinations";
                    }
                    else
                    {
                        StatusMessage = $"Failed to load destinations: {response.ErrorMessage}";
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading destinations: {ex.Message}";
            }
        }

        /// <summary>
        /// Demonstrates the Adapter pattern by importing legacy travel data
        /// </summary>
        private void ImportLegacyData()
        {
            try
            {
                // Create sample legacy data to demonstrate the adapter pattern
                var legacyData = new LegacyTravelData(
                    destName: "Paris",
                    countryCode: "FR", 
                    priceAmount: 1299.99m,
                    currencySymbol: "EUR",
                    departureTime: "2024-06-15 10:00:00",
                    returnTime: "2024-06-22 18:30:00",
                    totalSeats: 25,
                    occupiedSeats: 18,
                    travelType: "Cultural_Tour",
                    status: "confirmed"
                );

                // Use Adapter pattern to convert legacy data to modern format
                var adapter = new LegacyTravelAdapter(legacyData);
                var modernArrangement = adapter.ToTravelArrangement();

                // Add the converted arrangement to our collection
                AllArrangements.Add(modernArrangement);
                
                // Update UI
                ApplyFilters();
                UpdateChartData();
                
                // Show success message with adapter-specific information
                var legacyInfo = adapter.GetLegacyInfo();
                StatusMessage = $"Successfully imported legacy data using Adapter pattern. {legacyInfo}";
                
                MessageBox.Show(
                    $"Legacy data imported successfully!\n\n" +
                    $"Original destination: {legacyData.destination_name}\n" +
                    $"Converted to: {modernArrangement.Destination.TownName}\n" +
                    $"Price: {adapter.GetFormattedPrice()}\n" +
                    $"Available seats: {adapter.GetAvailableSeats()}\n" +
                    $"Duration: {adapter.GetTravelDuration()}\n" +
                    $"Status converted from '{legacyData.current_status}' to '{modernArrangement.State}'",
                    "Adapter Pattern Demo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error importing legacy data: {ex.Message}";
                MessageBox.Show($"Error importing legacy data: {ex.Message}", 
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}