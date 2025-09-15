using System.Collections.Generic;
using TravelSystem.Models.Enums;

namespace TravelSystem.Models.DTOs
{
    public class TravelArrangementDto
    {
        public string Id { get; set; }
        public ModeOfTransport AssociatedTransportation { get; set; }
        public int NumberOfDays { get; set; }
        public List<PassengerDto> Passengers { get; set; }
        public DestinationDto Destination { get; set; }
        public EntityState State { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public double TotalPrice { get; set; }

        public TravelArrangementDto()
        {
            Passengers = new List<PassengerDto>();
        }
    }
}
