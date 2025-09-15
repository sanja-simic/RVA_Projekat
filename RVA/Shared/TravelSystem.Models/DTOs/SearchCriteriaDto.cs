using TravelSystem.Models.Enums;

namespace TravelSystem.Models.DTOs
{
    public class SearchCriteriaDto
    {
        public string Destination { get; set; }
        public string DestinationCountry { get; set; }
        public string DestinationTown { get; set; }
        public ModeOfTransport? Transportation { get; set; }
        public EntityState? State { get; set; }
        public int? MinDays { get; set; }
        public int? MaxDays { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public double? MaxPricePerDay { get; set; }
    }
}
