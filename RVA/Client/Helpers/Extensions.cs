using System.Collections.Generic;
using System.Linq;
using TravelSystem.Models.DTOs;

namespace TravelSystem.Client.Helpers
{
    /// <summary>
    /// Extension methods for DTOs and other classes
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Creates a deep clone of TravelArrangementDto
        /// </summary>
        public static TravelArrangementDto Clone(this TravelArrangementDto source)
        {
            if (source == null) return null;

            return new TravelArrangementDto
            {
                Id = source.Id,
                AssociatedTransportation = source.AssociatedTransportation,
                NumberOfDays = source.NumberOfDays,
                State = source.State,
                CreatedAt = source.CreatedAt,
                UpdatedAt = source.UpdatedAt,
                TotalPrice = source.TotalPrice,
                Destination = source.Destination?.Clone(),
                Passengers = source.Passengers?.Select(p => p.Clone()).ToList() ?? new List<PassengerDto>()
            };
        }

        /// <summary>
        /// Creates a deep clone of DestinationDto
        /// </summary>
        public static DestinationDto Clone(this DestinationDto source)
        {
            if (source == null) return null;

            return new DestinationDto
            {
                Id = source.Id,
                TownName = source.TownName,
                CountryName = source.CountryName,
                StayPriceByDay = source.StayPriceByDay
            };
        }

        /// <summary>
        /// Creates a deep clone of PassengerDto
        /// </summary>
        public static PassengerDto Clone(this PassengerDto source)
        {
            if (source == null) return null;

            return new PassengerDto
            {
                Id = source.Id,
                FirstName = source.FirstName,
                LastName = source.LastName,
                PassportNumber = source.PassportNumber,
                LuggageWeight = source.LuggageWeight
            };
        }
    }
}