using System;
using System.Collections.Generic;
using System.Linq;
using TravelSystem.Models.DTOs;
using TravelSystem.Models.Enums;

namespace TravelSystem.Client.Helpers
{
    /// <summary>
    /// Helper class for searching and filtering travel arrangements
    /// </summary>
    public static class SearchHelper
    {
        /// <summary>
        /// Searches travel arrangements based on various criteria
        /// </summary>
        public static IEnumerable<TravelArrangementDto> SearchArrangements(
            IEnumerable<TravelArrangementDto> arrangements, 
            string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return arrangements;

            searchText = searchText.ToLower().Trim();

            return arrangements.Where(arrangement =>
                arrangement.Id?.ToLower().Contains(searchText) == true ||
                arrangement.Destination?.TownName?.ToLower().Contains(searchText) == true ||
                arrangement.Destination?.CountryName?.ToLower().Contains(searchText) == true ||
                arrangement.AssociatedTransportation.ToString().ToLower().Contains(searchText) ||
                arrangement.NumberOfDays.ToString().Contains(searchText) ||
                arrangement.TotalPrice.ToString().Contains(searchText) ||
                arrangement.State.ToString().ToLower().Contains(searchText) ||
                arrangement.CreatedAt.ToString("yyyy-MM-dd").Contains(searchText) ||
                arrangement.Passengers?.Any(p => 
                    p.FirstName?.ToLower().Contains(searchText) == true ||
                    p.LastName?.ToLower().Contains(searchText) == true ||
                    p.PassportNumber?.ToLower().Contains(searchText) == true) == true
            );
        }

        /// <summary>
        /// Filters arrangements by state
        /// </summary>
        public static IEnumerable<TravelArrangementDto> FilterByState(
            IEnumerable<TravelArrangementDto> arrangements, 
            EntityState? state)
        {
            if (!state.HasValue)
                return arrangements;

            return arrangements.Where(a => a.State == state.Value);
        }

        /// <summary>
        /// Filters arrangements by transportation mode
        /// </summary>
        public static IEnumerable<TravelArrangementDto> FilterByTransportation(
            IEnumerable<TravelArrangementDto> arrangements, 
            ModeOfTransport? transportation)
        {
            if (!transportation.HasValue)
                return arrangements;

            return arrangements.Where(a => a.AssociatedTransportation == transportation.Value);
        }

        /// <summary>
        /// Filters arrangements by date range
        /// </summary>
        public static IEnumerable<TravelArrangementDto> FilterByDateRange(
            IEnumerable<TravelArrangementDto> arrangements,
            DateTime? startDate,
            DateTime? endDate)
        {
            var filtered = arrangements;

            if (startDate.HasValue)
                filtered = filtered.Where(a => a.CreatedAt >= startDate.Value);

            if (endDate.HasValue)
                filtered = filtered.Where(a => a.CreatedAt <= endDate.Value);

            return filtered;
        }

        /// <summary>
        /// Filters arrangements by price range
        /// </summary>
        public static IEnumerable<TravelArrangementDto> FilterByPriceRange(
            IEnumerable<TravelArrangementDto> arrangements,
            double? minPrice,
            double? maxPrice)
        {
            var filtered = arrangements;

            if (minPrice.HasValue)
                filtered = filtered.Where(a => a.TotalPrice >= minPrice.Value);

            if (maxPrice.HasValue)
                filtered = filtered.Where(a => a.TotalPrice <= maxPrice.Value);

            return filtered;
        }
    }
}