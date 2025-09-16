using System;

namespace TravelSystem.Client.Legacy
{
    /// <summary>
    /// Represents a legacy travel data structure with incompatible format
    /// This class simulates an old system that needs to be integrated into the current application
    /// </summary>
    public class LegacyTravelData
    {
        // Legacy property names and formats
        public string destination_name { get; set; }
        public string country_code { get; set; }
        public decimal price_amount { get; set; }
        public string currency_symbol { get; set; }
        public string departure_timestamp { get; set; }
        public string return_timestamp { get; set; }
        public int total_seats { get; set; }
        public int occupied_seats { get; set; }
        public string travel_type { get; set; }
        public string current_status { get; set; }

        // Legacy constructor with old parameter names
        public LegacyTravelData(string destName, string countryCode, decimal priceAmount, 
            string currencySymbol, string departureTime, string returnTime, 
            int totalSeats, int occupiedSeats, string travelType, string status)
        {
            destination_name = destName;
            country_code = countryCode;
            price_amount = priceAmount;
            currency_symbol = currencySymbol;
            departure_timestamp = departureTime;
            return_timestamp = returnTime;
            total_seats = totalSeats;
            occupied_seats = occupiedSeats;
            travel_type = travelType;
            current_status = status;
        }

        // Legacy methods with different naming conventions
        public int GetAvailableSeats()
        {
            return total_seats - occupied_seats;
        }

        public string GetFormattedPrice()
        {
            return $"{price_amount} {currency_symbol}";
        }

        public bool IsFullyBooked()
        {
            return occupied_seats >= total_seats;
        }

        public string GetTravelDuration()
        {
            if (DateTime.TryParse(departure_timestamp, out DateTime departure) &&
                DateTime.TryParse(return_timestamp, out DateTime returnDate))
            {
                var duration = returnDate - departure;
                return $"{duration.Days} days";
            }
            return "Unknown duration";
        }
    }
}