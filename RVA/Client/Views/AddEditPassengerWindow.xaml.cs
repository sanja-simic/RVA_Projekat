using System;
using System.Windows;
using TravelSystem.Models.DTOs;

namespace TravelSystem.Client.Views
{
    public partial class AddEditPassengerWindow : Window
    {
        public PassengerDto Passenger { get; private set; }

        public AddEditPassengerWindow(PassengerDto passenger = null)
        {
            InitializeComponent();
            
            if (passenger != null)
            {
                Passenger = passenger;
                LoadPassenger(passenger);
                Title = "Edit Passenger";
            }
            else
            {
                Passenger = new PassengerDto();
                Title = "Add Passenger";
            }
        }

        private void LoadPassenger(PassengerDto passenger)
        {
            FirstNameTextBox.Text = passenger.FirstName;
            LastNameTextBox.Text = passenger.LastName;
            PassportNumberTextBox.Text = passenger.PassportNumber;
            LuggageWeightTextBox.Text = passenger.LuggageWeight.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                SavePassenger();
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
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
            {
                MessageBox.Show("Please enter a first name.", "Validation Error", 
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                FirstNameTextBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(LastNameTextBox.Text))
            {
                MessageBox.Show("Please enter a last name.", "Validation Error", 
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                LastNameTextBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(PassportNumberTextBox.Text))
            {
                MessageBox.Show("Please enter a passport number.", "Validation Error", 
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                PassportNumberTextBox.Focus();
                return false;
            }

            if (!int.TryParse(LuggageWeightTextBox.Text, out int luggageWeight) || luggageWeight < 0)
            {
                MessageBox.Show("Please enter a valid luggage weight.", "Validation Error", 
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                LuggageWeightTextBox.Focus();
                return false;
            }

            return true;
        }

        private void SavePassenger()
        {
            Passenger.FirstName = FirstNameTextBox.Text.Trim();
            Passenger.LastName = LastNameTextBox.Text.Trim();
            Passenger.PassportNumber = PassportNumberTextBox.Text.Trim();
            Passenger.LuggageWeight = int.Parse(LuggageWeightTextBox.Text);
            
            if (string.IsNullOrEmpty(Passenger.Id))
            {
                Passenger.Id = Guid.NewGuid().ToString();
            }
        }
    }
}