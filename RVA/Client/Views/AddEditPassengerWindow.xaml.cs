using System;
using System.Windows;
using TravelSystem.Client.ViewModels;
using TravelSystem.Models.DTOs;

namespace TravelSystem.Client.Views
{
    public partial class AddEditPassengerWindow : Window
    {
        private AddEditPassengerViewModel _viewModel;

        public PassengerDto Passenger => _viewModel?.Result;

        public AddEditPassengerWindow(PassengerDto passenger = null)
        {
            InitializeComponent();
            
            _viewModel = new AddEditPassengerViewModel(passenger);
            DataContext = _viewModel;
            
            _viewModel.SaveCompleted += OnSaveCompleted;
        }

        private void OnSaveCompleted(object sender, bool saved)
        {
            DialogResult = saved;
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.SaveCompleted -= OnSaveCompleted;
            }
            base.OnClosed(e);
        }
    }
}