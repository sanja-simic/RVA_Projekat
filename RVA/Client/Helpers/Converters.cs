using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TravelSystem.Models.Enums;

namespace TravelSystem.Client.Helpers
{
    /// <summary>
    /// Converter for boolean to connection status text
    /// </summary>
    public class BoolToConnectionStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isConnected)
            {
                return isConnected ? "Connected" : "Disconnected";
            }
            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter for boolean to connection status color
    /// </summary>
    public class BoolToConnectionColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isConnected)
            {
                return isConnected ? Brushes.Green : Brushes.Red;
            }
            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Helper class for entity state filter items
    /// </summary>
    public class EntityStateFilter
    {
        public string DisplayName { get; set; }
        public EntityState? Value { get; set; }
    }

    /// <summary>
    /// Helper class for transportation filter items
    /// </summary>
    public class TransportationFilter
    {
        public string DisplayName { get; set; }
        public ModeOfTransport? Value { get; set; }
    }
}