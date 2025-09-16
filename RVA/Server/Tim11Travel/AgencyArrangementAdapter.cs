using System;using System;using System;

using TravelSystem.Models.Entities;

using TravelSystem.Models.Enums;using System.Collections.Generic;using System.Collections.Generic;



namespace Tim11.Travelusing System.Text;using System.Text;

{

	public class AgencyArrangementAdapterusing TravelSystem.Models.Entities;using TravelSystem.Models.Entities;

	{

		private AgencyArrangement agencyArrangement;using TravelSystem.Models.Enums;using TravelSystem.Models.Enums;



		public AgencyArrangementAdapter(AgencyArrangement agencyArrangement)

		{

			this.agencyArrangement = agencyArrangement;namespace Tim11.Travelnamespace Tim11.Travel

		}

{{

		public TravelArrangement ConvertToTravelArrangement()

		{	public class AgencyArrangementAdapter	public class AgencyArrangementAdapter

			// Convert AgencyArrangement to TravelArrangement

			var destination = new Destination(	{	{

				Guid.NewGuid().ToString(),

				agencyArrangement.GetLocation(),		private AgencyArrangement agencyArrangement;		private ModeOfTransport ConvertTransportation(string agencyTransportation)

				agencyArrangement.GetDestination(),

				100.0 // default price		{

			);

		public AgencyArrangementAdapter(AgencyArrangement agencyArrangement)			switch (agencyTransportation.ToUpper())

			var transport = ConvertTransportation(agencyArrangement.GetTransport());

					{			{

			var travelArrangement = new TravelArrangement(

				agencyArrangement.Id,			this.agencyArrangement = agencyArrangement;				case "AIRPLANE":

				transport,

				CalculateDays(),		}					return ModeOfTransport.Plane;

				destination

			);				case "CAR":



			return travelArrangement;		public TravelArrangement ConvertToTravelArrangement()					return ModeOfTransport.Car;

		}

		{				case "BUS":

		private ModeOfTransport ConvertTransportation(string agencyTransportation)

		{			// Konvertujemo AgencyArrangement u TravelArrangement					return ModeOfTransport.Bus;

			switch (agencyTransportation.ToUpper())

			{			var destination = new Destination(				case "SHIP":

				case "AIRPLANE":

					return ModeOfTransport.Plane;				Guid.NewGuid().ToString(),					return ModeOfTransport.Ship;

				case "CAR":

					return ModeOfTransport.Car;				agencyArrangement.GetLocation(),				case "TRAIN":

				case "BUS":

					return ModeOfTransport.Bus;				agencyArrangement.GetDestination(),					return ModeOfTransport.Train;

				case "SHIP":

					return ModeOfTransport.Ship;				100.0 // default price				default:

				case "TRAIN":

					return ModeOfTransport.Train;			);					return ModeOfTransport.Car; // Default

				default:

					return ModeOfTransport.Car;			}

			}

		}			var transport = ConvertTransportation(agencyArrangement.GetTransport());		}ng TravelSystem.Models.Entities;



		private int CalculateDays()			using TravelSystem.Models.Enums;

		{

			return (agencyArrangement.GetReturnDate() - agencyArrangement.GetDepartureDate()).Days;			var travelArrangement = new TravelArrangement(

		}

	}				agencyArrangement.Id,namespace Tim11.Travel

}
				transport,{

				CalculateDays(),	public class AgencyArrangementAdapter

				destination	{

			);		private AgencyArrangement agencyArrangement;



			return travelArrangement;		public AgencyArrangementAdapter(AgencyArrangement agencyArrangement)

		}		{

			this.agencyArrangement = agencyArrangement;

		public static AgencyArrangement ConvertFromTravelArrangement(TravelArrangement travelArrangement)		}

		{

			// Konvertujemo TravelArrangement u AgencyArrangement		public TravelArrangement ConvertToTravelArrangement()

			return new AgencyArrangement(		{

				travelArrangement.Id,			// Konvertujemo AgencyArrangement u TravelArrangement

				travelArrangement.Destination.CountryName,			var destination = new Destination(

				travelArrangement.AssociatedTransportation.ToString(),				Guid.NewGuid().ToString(),

				travelArrangement.Destination.TownName,				agencyArrangement.GetLocation(),

				50, // default luggage limit				agencyArrangement.GetDestination(),

				"DefaultClient",				100.0 // default price

				DateTime.Now,			);

				DateTime.Now.AddDays(travelArrangement.NumberOfDays),

				travelArrangement.NumberOfDays * travelArrangement.Destination.StayPriceByDay			var transport = ConvertTransportMode(agencyArrangement.GetTransport());

			);			

		}			var travelArrangement = new TravelArrangement(

				agencyArrangement.Id,

		private ModeOfTransport ConvertTransportation(string agencyTransportation)				transport,

		{				CalculateDays(),

			switch (agencyTransportation.ToUpper())				destination

			{			);

				case "AIRPLANE":

					return ModeOfTransport.Plane;			return travelArrangement;

				case "CAR":		}

					return ModeOfTransport.Car;

				case "BUS":		public static AgencyArrangement ConvertFromTravelArrangement(TravelArrangement travelArrangement)

					return ModeOfTransport.Bus;		{

				case "SHIP":			// Konvertujemo TravelArrangement u AgencyArrangement

					return ModeOfTransport.Ship;			return new AgencyArrangement(

				case "TRAIN":				travelArrangement.Id,

					return ModeOfTransport.Train;				travelArrangement.Destination.CountryName,

				default:				travelArrangement.AssociatedTransportation.ToString(),

					return ModeOfTransport.Car; // Default				travelArrangement.Destination.TownName,

			}				50, // default luggage limit

		}				"DefaultClient",

				DateTime.Now,

		private int CalculateDays()				DateTime.Now.AddDays(travelArrangement.NumberOfDays),

		{				travelArrangement.NumberOfDays * travelArrangement.Destination.StayPriceByDay

			return (agencyArrangement.GetReturnDate() - agencyArrangement.GetDepartureDate()).Days;			);

		}		}

	}

}		private ModeOfTransport ConvertTransportMode(string transport)
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
