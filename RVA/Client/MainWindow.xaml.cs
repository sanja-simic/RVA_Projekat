using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;

namespace TravelSystem.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StatusTextBlock.Text = "WPF Client is ready - Service integration will be added next";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Add functionality will be implemented after server integration.", "Information", 
                           MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Edit functionality will be implemented after server integration.", 
                           "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Delete functionality will be implemented after server integration.", 
                           "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            StatusTextBlock.Text = "Refresh clicked - server integration needed";
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            StatusTextBlock.Text = "Connect functionality will be implemented after basic WPF setup";
        }
    }
}
