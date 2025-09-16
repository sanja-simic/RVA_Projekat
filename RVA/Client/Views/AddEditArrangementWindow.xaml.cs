using System;
using System.Windows;
using TravelSystem.Client.ViewModels;
using TravelSystem.Models.DTOs;

namespace TravelSystem.Client.Views
{
    public partial class AddEditArrangementWindow : Window
    {
        private AddEditArrangementViewModel _viewModel;

        public TravelArrangementDto Arrangement => _viewModel?.Result;

        public AddEditArrangementWindow(TravelArrangementDto arrangement = null)
        {
            InitializeComponent();
            
            _viewModel = new AddEditArrangementViewModel(arrangement);
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
                _viewModel.Dispose();
            }
            base.OnClosed(e);
        }
    }
}