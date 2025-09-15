using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TravelSystem.Client.Services;
using TravelSystem.Client.Views;
using TravelSystem.Models.DTOs;

namespace TravelSystem.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TravelServiceClient serviceClient;
        private ObservableCollection<TravelArrangementDto> arrangements;

        public MainWindow()
        {
            InitializeComponent();
            arrangements = new ObservableCollection<TravelArrangementDto>();
            TravelArrangementsDataGrid.ItemsSource = arrangements;
            
            // Initialize service connection after a delay to allow server startup
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(3000); // Give server time to start
            InitializeServiceConnection();
        }

        private void InitializeServiceConnection()
        {
            try
            {
                serviceClient = new TravelServiceClient();
                if (serviceClient.IsConnected)
                {
                    StatusTextBlock.Text = "Connected to server";
                    RefreshArrangements();
                }
                else
                {
                    StatusTextBlock.Text = "Failed to connect to server";
                }
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Connection error: {ex.Message}";
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var addWindow = new AddEditArrangementWindow();
                if (addWindow.ShowDialog() == true)
                {
                    var newArrangement = addWindow.Arrangement;
                    var response = serviceClient.Service.AddArrangement(newArrangement);
                    
                    if (response.IsSuccess)
                    {
                        arrangements.Add(response.Data);
                        StatusTextBlock.Text = "Arrangement added successfully";
                    }
                    else
                    {
                        MessageBox.Show($"Failed to add arrangement: {response.ErrorMessage}", "Error", 
                                       MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding arrangement: {ex.Message}", "Error", 
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedArrangement = TravelArrangementsDataGrid.SelectedItem as TravelArrangementDto;
            if (selectedArrangement == null)
            {
                MessageBox.Show("Please select an arrangement to edit.", "No Selection", 
                               MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var editWindow = new AddEditArrangementWindow(selectedArrangement);
                if (editWindow.ShowDialog() == true)
                {
                    var updatedArrangement = editWindow.Arrangement;
                    var response = serviceClient.Service.UpdateArrangement(updatedArrangement);
                    
                    if (response.IsSuccess)
                    {
                        RefreshArrangements();
                        StatusTextBlock.Text = "Arrangement updated successfully";
                    }
                    else
                    {
                        MessageBox.Show($"Failed to update arrangement: {response.ErrorMessage}", "Error", 
                                       MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating arrangement: {ex.Message}", "Error", 
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedArrangement = TravelArrangementsDataGrid.SelectedItem as TravelArrangementDto;
            if (selectedArrangement == null)
            {
                MessageBox.Show("Please select an arrangement to delete.", "No Selection", 
                               MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete the arrangement to {selectedArrangement.Destination.TownName}?", 
                                        "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var response = serviceClient.Service.DeleteArrangement(selectedArrangement.Id);
                    
                    if (response.IsSuccess)
                    {
                        arrangements.Remove(selectedArrangement);
                        StatusTextBlock.Text = "Arrangement deleted successfully";
                    }
                    else
                    {
                        MessageBox.Show($"Failed to delete arrangement: {response.ErrorMessage}", "Error", 
                                       MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting arrangement: {ex.Message}", "Error", 
                                   MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshArrangements();
        }

        private void RefreshArrangements()
        {
            try
            {
                if (serviceClient?.IsConnected == true)
                {
                    var response = serviceClient.Service.GetAllArrangements();
                    
                    if (response.IsSuccess)
                    {
                        arrangements.Clear();
                        foreach (var arrangement in response.Data)
                        {
                            arrangements.Add(arrangement);
                        }
                        StatusTextBlock.Text = $"Loaded {arrangements.Count} arrangements";
                    }
                    else
                    {
                        StatusTextBlock.Text = $"Failed to load arrangements: {response.ErrorMessage}";
                    }
                }
                else
                {
                    StatusTextBlock.Text = "Not connected to server";
                }
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Error refreshing: {ex.Message}";
            }
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (serviceClient == null)
                {
                    InitializeServiceConnection();
                }
                else
                {
                    serviceClient.Reconnect();
                    StatusTextBlock.Text = serviceClient.IsConnected ? "Reconnected to server" : "Failed to reconnect";
                }

                if (serviceClient?.IsConnected == true)
                {
                    RefreshArrangements();
                }
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Connection error: {ex.Message}";
            }
        }

        private void TravelArrangementsDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedArrangement = TravelArrangementsDataGrid.SelectedItem as TravelArrangementDto;
            if (selectedArrangement != null)
            {
                try
                {
                    var response = serviceClient.Service.ChangeArrangementState(selectedArrangement.Id);
                    
                    if (response.IsSuccess)
                    {
                        RefreshArrangements();
                        StatusTextBlock.Text = $"State changed to {response.Data.State}";
                    }
                    else
                    {
                        MessageBox.Show($"Failed to change state: {response.ErrorMessage}", "Error", 
                                       MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error changing state: {ex.Message}", "Error", 
                                   MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            serviceClient?.Dispose();
            base.OnClosed(e);
        }
    }
}
