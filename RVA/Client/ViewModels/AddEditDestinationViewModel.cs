using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TravelSystem.Client.Commands;
using TravelSystem.Models.DTOs;

namespace TravelSystem.Client.ViewModels
{
    /// <summary>
    /// ViewModel for adding and editing destinations with validation
    /// </summary>
    public class AddEditDestinationViewModel : BaseViewModel, IDataErrorInfo
    {
        private readonly bool _isEditMode;

        // Properties for binding
        private string _id;
        private string _townName;
        private string _countryName;
        private double _stayPriceByDay;

        // Validation errors
        private readonly Dictionary<string, string> _errors = new Dictionary<string, string>();

        public AddEditDestinationViewModel(DestinationDto destination = null)
        {
            _isEditMode = destination != null;

            // Initialize commands
            SaveCommand = new RelayCommand(_ => Save(), _ => CanSave);
            CancelCommand = new RelayCommand(_ => Cancel());

            // Initialize from existing destination if editing
            if (_isEditMode)
            {
                LoadDestination(destination);
            }
            else
            {
                // Set defaults for new destination
                _id = Guid.NewGuid().ToString();
                _stayPriceByDay = 50.0; // Default price per day
            }
        }

        #region Properties

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string TownName
        {
            get => _townName;
            set
            {
                if (SetProperty(ref _townName, value))
                {
                    ValidateProperty(nameof(TownName));
                }
            }
        }

        public string CountryName
        {
            get => _countryName;
            set
            {
                if (SetProperty(ref _countryName, value))
                {
                    ValidateProperty(nameof(CountryName));
                }
            }
        }

        public double StayPriceByDay
        {
            get => _stayPriceByDay;
            set
            {
                if (SetProperty(ref _stayPriceByDay, value))
                {
                    ValidateProperty(nameof(StayPriceByDay));
                }
            }
        }

        public bool IsEditMode => _isEditMode;
        public string WindowTitle => _isEditMode ? "Edit Destination" : "Add New Destination";

        public bool CanSave => !HasErrors && 
                              !string.IsNullOrWhiteSpace(TownName) && 
                              !string.IsNullOrWhiteSpace(CountryName) && 
                              StayPriceByDay > 0;

        public bool HasErrors => _errors.Any();

        #endregion

        #region Commands

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

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

        private void LoadDestination(DestinationDto destination)
        {
            Id = destination.Id;
            TownName = destination.TownName;
            CountryName = destination.CountryName;
            StayPriceByDay = destination.StayPriceByDay;
        }

        private void ValidateProperty(string propertyName)
        {
            _errors.Remove(propertyName);

            switch (propertyName)
            {
                case nameof(TownName):
                    if (string.IsNullOrWhiteSpace(TownName))
                        _errors[propertyName] = "Town name is required";
                    else if (TownName.Length > 100)
                        _errors[propertyName] = "Town name cannot exceed 100 characters";
                    break;

                case nameof(CountryName):
                    if (string.IsNullOrWhiteSpace(CountryName))
                        _errors[propertyName] = "Country name is required";
                    else if (CountryName.Length > 100)
                        _errors[propertyName] = "Country name cannot exceed 100 characters";
                    break;

                case nameof(StayPriceByDay):
                    if (StayPriceByDay <= 0)
                        _errors[propertyName] = "Price per day must be greater than 0";
                    else if (StayPriceByDay > 10000)
                        _errors[propertyName] = "Price per day cannot exceed 10,000";
                    break;
            }

            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(nameof(CanSave));
        }

        private void Save()
        {
            var destination = new DestinationDto
            {
                Id = Id,
                TownName = TownName,
                CountryName = CountryName,
                StayPriceByDay = StayPriceByDay
            };

            Result = destination;
            OnSaveCompleted(true);
        }

        private void Cancel()
        {
            OnSaveCompleted(false);
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

        public DestinationDto Result { get; private set; }

        #endregion
    }
}