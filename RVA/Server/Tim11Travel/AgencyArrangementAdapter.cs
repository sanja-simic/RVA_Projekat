using System;
using System.Collections.Generic;
using System.Text;

namespace Tim11.Travel
{
	public class AgencyArrangementAdapter
	{
		private AgencyArrangement agencyArrangement;

		public AgencyArrangementAdapter(AgencyArrangement agencyArrangement)
		{
			this.agencyArrangement = agencyArrangement;
		}

		public TravelArrangement ConvertToTravelArrangement()
		{
			// Konvertujemo AgencyArrangement u TravelArrangement
			var destination = new Destination(
				Guid.NewGuid().ToString(),
				agencyArrangement.GetLocation(),
				agencyArrangement.GetDestination(),
				100.0 // default price
			);

			var transport = ConvertTransportMode(agencyArrangement.GetTransport());
			
			var travelArrangement = new TravelArrangement(
				agencyArrangement.Id,
				transport,
				CalculateDays(),
				destination
			);

			return travelArrangement;
		}

		public static AgencyArrangement ConvertFromTravelArrangement(TravelArrangement travelArrangement)
		{
			// Konvertujemo TravelArrangement u AgencyArrangement
			return new AgencyArrangement(
				travelArrangement.Id,
				travelArrangement.Destination.CountryName,
				travelArrangement.AssociatedTransportation.ToString(),
				travelArrangement.Destination.TownName,
				50, // default luggage limit
				"DefaultClient",
				DateTime.Now,
				DateTime.Now.AddDays(travelArrangement.NumberOfDays),
				travelArrangement.NumberOfDays * travelArrangement.Destination.StayPriceByDay
			);
		}

		private ModeOfTransport ConvertTransportMode(string transport)
		{
			switch (transport.ToUpper())
			{
				case "AIRPLANE":
					return ModeOfTransport.AIRPLANE;
				case "CAR":
					return ModeOfTransport.CAR;
				case "BUS":
					return ModeOfTransport.BUS;
				case "SHIP":
					return ModeOfTransport.SHIP;
				case "TRAIN":
					return ModeOfTransport.TRAIN;
				default:
					return ModeOfTransport.CAR;
			}
		}

		private int CalculateDays()
		{
			return (agencyArrangement.GetReturnDate() - agencyArrangement.GetDepartureDate()).Days;
		}
	}
}
