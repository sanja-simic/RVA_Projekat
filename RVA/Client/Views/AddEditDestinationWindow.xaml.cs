using System;
using System.Windows;
using TravelSystem.Client.ViewModels;
using TravelSystem.Models.DTOs;

namespace TravelSystem.Client.Views
{
    public partial class AddEditDestinationWindow : Window
    {
        private AddEditDestinationViewModel _viewModel;

        public DestinationDto Destination => _viewModel?.Result;

        public AddEditDestinationWindow(DestinationDto destination = null)
        {
            InitializeComponent();
            
            _viewModel = new AddEditDestinationViewModel(destination);
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