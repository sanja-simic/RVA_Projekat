using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TravelSystem.Models.DTOs;
using TravelSystem.Models.Enums;

namespace TravelSystem.Client.Views
{
    public partial class AddEditArrangementWindow : Window
    {
        public TravelArrangementDto Arrangement { get; private set; }
        private ObservableCollection<PassengerDto> passengers;
        private bool isEditMode;

        public AddEditArrangementWindow(TravelArrangementDto arrangement = null)
        {
            InitializeComponent();
            
            isEditMode = arrangement != null;
            passengers = new ObservableCollection<PassengerDto>();
            PassengersDataGrid.ItemsSource = passengers;
            
            InitializeControls();
            
            if (isEditMode)
            {
                LoadArrangement(arrangement);
                Title = "Edit Travel Arrangement";
            }
            else
            {
                Title = "Add Travel Arrangement";
                Arrangement = new TravelArrangementDto();
            }
            
            UpdateTotalPrice();
        }

        private void InitializeControls()
        {
            // Load transportation modes
            TransportationComboBox.ItemsSource = Enum.GetValues(typeof(ModeOfTransport));
            TransportationComboBox.SelectedIndex = 0;
            
            // Event handlers for price calculation
            NumberOfDaysTextBox.TextChanged += (s, e) => UpdateTotalPrice();
            PricePerDayTextBox.TextChanged += (s, e) => UpdateTotalPrice();
        }

        private void LoadArrangement(TravelArrangementDto arrangement)
        {
            Arrangement = arrangement;
            
            // Load destination data
            TownTextBox.Text = arrangement.Destination.TownName;
            CountryTextBox.Text = arrangement.Destination.CountryName;
            PricePerDayTextBox.Text = arrangement.Destination.StayPriceByDay.ToString();
            
            // Load travel details
            TransportationComboBox.SelectedItem = arrangement.AssociatedTransportation;
            NumberOfDaysTextBox.Text = arrangement.NumberOfDays.ToString();
            
            // Load passengers
            if (arrangement.Passengers != null)
            {
                foreach (var passenger in arrangement.Passengers)
                {
                    passengers.Add(passenger);
                }
            }
        }

        private void UpdateTotalPrice()
        {
            if (double.TryParse(PricePerDayTextBox.Text, out double pricePerDay) &&
                int.TryParse(NumberOfDaysTextBox.Text, out int numberOfDays))
            {
                double totalPrice = pricePerDay * numberOfDays;
                TotalPriceTextBlock.Text = totalPrice.ToString("C");
            }
            else
            {
                TotalPriceTextBlock.Text = "0.00";
            }
        }

        private void AddPassengerButton_Click(object sender, RoutedEventArgs e)
        {
            var passengerWindow = new AddEditPassengerWindow();
            if (passengerWindow.ShowDialog() == true)
            {
                passengers.Add(passengerWindow.Passenger);
            }
        }

        private void RemovePassengerButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPassenger = PassengersDataGrid.SelectedItem as PassengerDto;
            if (selectedPassenger != null)
            {
                passengers.Remove(selectedPassenger);
            }
            else
            {
                MessageBox.Show("Please select a passenger to remove.", "No Selection", 
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                SaveArrangement();
                DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(TownTextBox.Text))
            {
                MessageBox.Show("Please enter a town name.", "Validation Error", 
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                TownTextBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(CountryTextBox.Text))
            {
                MessageBox.Show("Please enter a country name.", "Validation Error", 
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                CountryTextBox.Focus();
                return false;
            }

            if (!double.TryParse(PricePerDayTextBox.Text, out double pricePerDay) || pricePerDay <= 0)
            {
                MessageBox.Show("Please enter a valid price per day.", "Validation Error", 
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                PricePerDayTextBox.Focus();
                return false;
            }

            if (!int.TryParse(NumberOfDaysTextBox.Text, out int numberOfDays) || numberOfDays <= 0)
            {
                MessageBox.Show("Please enter a valid number of days.", "Validation Error", 
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                NumberOfDaysTextBox.Focus();
                return false;
            }

            if (TransportationComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a mode of transportation.", "Validation Error", 
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                TransportationComboBox.Focus();
                return false;
            }

            return true;
        }

        private void SaveArrangement()
        {
            // Create or update destination
            var destination = new DestinationDto
            {
                Id = isEditMode ? Arrangement.Destination.Id : Guid.NewGuid().ToString(),
                TownName = TownTextBox.Text.Trim(),
                CountryName = CountryTextBox.Text.Trim(),
                StayPriceByDay = double.Parse(PricePerDayTextBox.Text)
            };

            // Update or create arrangement
            if (!isEditMode)
            {
                Arrangement = new TravelArrangementDto();
            }

            Arrangement.Destination = destination;
            Arrangement.AssociatedTransportation = (ModeOfTransport)TransportationComboBox.SelectedItem;
            Arrangement.NumberOfDays = int.Parse(NumberOfDaysTextBox.Text);
            Arrangement.Passengers = passengers.ToList();
            Arrangement.TotalPrice = destination.StayPriceByDay * Arrangement.NumberOfDays;
            
            if (!isEditMode)
            {
                Arrangement.State = EntityState.Reserved;
                Arrangement.CreatedAt = DateTime.Now;
            }
            
            Arrangement.UpdatedAt = DateTime.Now;
        }
    }
}