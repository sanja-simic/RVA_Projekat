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
    /// ViewModel for adding and editing passengers with validation
    /// </summary>
    public class AddEditPassengerViewModel : BaseViewModel, IDataErrorInfo
    {
        private readonly bool _isEditMode;

        // Properties for binding
        private string _id;
        private string _firstName;
        private string _lastName;
        private string _passportNumber;
        private int _luggageWeight;

        // Validation errors
        private readonly Dictionary<string, string> _errors = new Dictionary<string, string>();

        public AddEditPassengerViewModel(PassengerDto passenger = null)
        {
            _isEditMode = passenger != null;

            // Initialize commands
            SaveCommand = new RelayCommand(_ => Save(), _ => CanSave);
            CancelCommand = new RelayCommand(_ => Cancel());

            // Initialize from existing passenger if editing
            if (_isEditMode)
            {
                LoadPassenger(passenger);
            }
            else
            {
                // Set defaults for new passenger
                _id = Guid.NewGuid().ToString();
                _luggageWeight = 0;
            }
        }

        #region Properties

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (SetProperty(ref _firstName, value))
                {
                    ValidateProperty(nameof(FirstName));
                }
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (SetProperty(ref _lastName, value))
                {
                    ValidateProperty(nameof(LastName));
                }
            }
        }

        public string PassportNumber
        {
            get => _passportNumber;
            set
            {
                if (SetProperty(ref _passportNumber, value))
                {
                    ValidateProperty(nameof(PassportNumber));
                }
            }
        }

        public int LuggageWeight
        {
            get => _luggageWeight;
            set
            {
                if (SetProperty(ref _luggageWeight, value))
                {
                    ValidateProperty(nameof(LuggageWeight));
                }
            }
        }

        public bool IsEditMode => _isEditMode;
        public string WindowTitle => _isEditMode ? "Edit Passenger" : "Add New Passenger";

        public bool CanSave => !HasErrors && 
                              !string.IsNullOrWhiteSpace(FirstName) && 
                              !string.IsNullOrWhiteSpace(LastName) && 
                              !string.IsNullOrWhiteSpace(PassportNumber);

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

        private void LoadPassenger(PassengerDto passenger)
        {
            Id = passenger.Id;
            FirstName = passenger.FirstName;
            LastName = passenger.LastName;
            PassportNumber = passenger.PassportNumber;
            LuggageWeight = passenger.LuggageWeight;
        }

        private void ValidateProperty(string propertyName)
        {
            _errors.Remove(propertyName);

            switch (propertyName)
            {
                case nameof(FirstName):
                    if (string.IsNullOrWhiteSpace(FirstName))
                        _errors[propertyName] = "First name is required";
                    else if (FirstName.Length > 50)
                        _errors[propertyName] = "First name cannot exceed 50 characters";
                    break;

                case nameof(LastName):
                    if (string.IsNullOrWhiteSpace(LastName))
                        _errors[propertyName] = "Last name is required";
                    else if (LastName.Length > 50)
                        _errors[propertyName] = "Last name cannot exceed 50 characters";
                    break;

                case nameof(PassportNumber):
                    if (string.IsNullOrWhiteSpace(PassportNumber))
                        _errors[propertyName] = "Passport number is required";
                    else if (PassportNumber.Length < 6 || PassportNumber.Length > 20)
                        _errors[propertyName] = "Passport number must be between 6 and 20 characters";
                    break;

                case nameof(LuggageWeight):
                    if (LuggageWeight < 0)
                        _errors[propertyName] = "Luggage weight cannot be negative";
                    else if (LuggageWeight > 50)
                        _errors[propertyName] = "Luggage weight cannot exceed 50 kg";
                    break;
            }

            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(nameof(CanSave));
        }

        private void Save()
        {
            var passenger = new PassengerDto
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                PassportNumber = PassportNumber,
                LuggageWeight = LuggageWeight
            };

            Result = passenger;
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

        public PassengerDto Result { get; private set; }

        #endregion
    }
}