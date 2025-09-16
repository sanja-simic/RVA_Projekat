using System;
using System.Globalization;
using TravelSystem.Client.Legacy;
using TravelSystem.Models.DTOs;
using TravelSystem.Models.Enums;

namespace TravelSystem.Client.Adapters
{
    /// <summary>
    /// Adapter pattern implementation to integrate legacy travel data into the current system
    /// Converts between incompatible LegacyTravelData format and modern TravelArrangementDTO
    /// </summary>
    public class LegacyTravelAdapter
    {
        private readonly LegacyTravelData _legacyData;

        public LegacyTravelAdapter(LegacyTravelData legacyData)
        {
            _legacyData = legacyData ?? throw new ArgumentNullException(nameof(legacyData));
        }

        /// <summary>
        /// Converts legacy travel data to modern TravelArrangementDto format
        /// </summary>
        /// <returns>TravelArrangementDto with converted data</returns>
        public TravelArrangementDto ToTravelArrangement()
        {
            return new TravelArrangementDto
            {
                Id = GenerateIdFromLegacyData().ToString(),
                TotalPrice = (double)_legacyData.price_amount,
                NumberOfDays = CalculateDaysFromLegacyData(),
                AssociatedTransportation = ConvertToModeOfTransport(_legacyData.travel_type),
                State = ConvertLegacyStatusToEntityState(_legacyData.current_status),
                Destination = CreateDestinationFromLegacy(),
                Passengers = new System.Collections.Generic.List<PassengerDto>(), // Empty list for legacy data
                CreatedAt = ParseLegacyDate(_legacyData.departure_timestamp),
                UpdatedAt = System.DateTime.Now
            };
        }

        /// <summary>
        /// Creates a destination DTO from legacy data
        /// </summary>
        private DestinationDto CreateDestinationFromLegacy()
        {
            return new DestinationDto
            {
                Id = GenerateDestinationId().ToString(),
                TownName = _legacyData.destination_name,
                CountryName = _legacyData.country_code,
                StayPriceByDay = 100.0 // Default value for legacy data
            };
        }

        /// <summary>
        /// Calculates number of days from legacy data
        /// </summary>
        private int CalculateDaysFromLegacyData()
        {
            var departure = ParseLegacyDate(_legacyData.departure_timestamp);
            var returnDate = ParseLegacyDate(_legacyData.return_timestamp);
            var duration = returnDate - departure;
            return Math.Max(1, duration.Days); // Minimum 1 day
        }

        /// <summary>
        /// Converts legacy travel type to ModeOfTransport enum
        /// </summary>
        private ModeOfTransport ConvertToModeOfTransport(string travelType)
        {
            if (string.IsNullOrEmpty(travelType))
                return ModeOfTransport.Plane; // Default

            switch (travelType.ToLower().Replace("_", ""))
            {
                case "bus":
                case "bustour":
                    return ModeOfTransport.Bus;
                case "plane":
                case "flight":
                case "air":
                    return ModeOfTransport.Plane;
                case "ship":
                case "cruise":
                case "boat":
                    return ModeOfTransport.Ship;
                default:
                    return ModeOfTransport.Plane; // Default fallback
            }
        }

        /// <summary>
        /// Converts legacy status string to modern EntityState enum
        /// </summary>
        private EntityState ConvertLegacyStatusToEntityState(string legacyStatus)
        {
            if (string.IsNullOrEmpty(legacyStatus))
                return EntityState.Reserved;

            switch (legacyStatus.ToLower())
            {
                case "reserved":
                case "booking":
                    return EntityState.Reserved;
                case "confirmed":
                case "paid":
                    return EntityState.Paid;
                case "active":
                case "in_progress":
                case "ongoing":
                    return EntityState.InProgress;
                case "completed":
                case "finished":
                case "done":
                    return EntityState.Completed;
                case "deleted":
                case "cancelled":
                case "inactive":
                    return EntityState.Cancelled;
                default:
                    return EntityState.Reserved;
            }
        }

        /// <summary>
        /// Parses legacy timestamp format to DateTime
        /// </summary>
        private DateTime ParseLegacyDate(string legacyTimestamp)
        {
            if (DateTime.TryParse(legacyTimestamp, out DateTime result))
                return result;

            // Try different legacy date formats
            string[] formats = { 
                "yyyy-MM-dd HH:mm:ss", 
                "MM/dd/yyyy", 
                "dd.MM.yyyy",
                "yyyy-MM-dd"
            };

            foreach (var format in formats)
            {
                if (DateTime.TryParseExact(legacyTimestamp, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                    return result;
            }

            // Default to today if parsing fails
            return DateTime.Today;
        }

        /// <summary>
        /// Generates a unique ID based on legacy data
        /// </summary>
        private int GenerateIdFromLegacyData()
        {
            // Create a simple hash-based ID from legacy data
            var hashSource = $"{_legacyData.destination_name}{_legacyData.departure_timestamp}{_legacyData.price_amount}";
            return Math.Abs(hashSource.GetHashCode()) % 100000; // Limit to 5 digits
        }

        /// <summary>
        /// Generates destination ID from legacy data
        /// </summary>
        private int GenerateDestinationId()
        {
            var hashSource = $"{_legacyData.destination_name}{_legacyData.country_code}";
            return Math.Abs(hashSource.GetHashCode()) % 10000; // Limit to 4 digits
        }

        /// <summary>
        /// Gets available seats using legacy method
        /// </summary>
        public int GetAvailableSeats()
        {
            return _legacyData.GetAvailableSeats();
        }

        /// <summary>
        /// Gets formatted price using legacy method
        /// </summary>
        public string GetFormattedPrice()
        {
            return _legacyData.GetFormattedPrice();
        }

        /// <summary>
        /// Checks if arrangement is fully booked using legacy method
        /// </summary>
        public bool IsFullyBooked()
        {
            return _legacyData.IsFullyBooked();
        }

        /// <summary>
        /// Gets travel duration using legacy method
        /// </summary>
        public string GetTravelDuration()
        {
            return _legacyData.GetTravelDuration();
        }

        /// <summary>
        /// Gets additional info specific to legacy data
        /// </summary>
        public string GetLegacyInfo()
        {
            return $"Legacy Travel Type: {_legacyData.travel_type}, " +
                   $"Available Seats: {GetAvailableSeats()}, " +
                   $"Duration: {GetTravelDuration()}";
        }
    }
}