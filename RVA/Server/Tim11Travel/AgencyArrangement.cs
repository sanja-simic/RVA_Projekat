using System;
using System.Collections.Generic;
using System.Text;

namespace Tim11.Travel
{
	public class AgencyArrangement : AgencyTravel
	{
		private string client;
		private DateTime departureDate;
		private DateTime returnDate;
		private double overallPrice;

		public AgencyArrangement(string id, string destination, string prefferedTransport, string travelLocation, int luggageLimit, 
			string client, DateTime departureDate, DateTime returnDate, double overallPrice)
			: base(id, destination, prefferedTransport, travelLocation, luggageLimit)
		{
			this.client = client;
			this.departureDate = departureDate;
			this.returnDate = returnDate;
			this.overallPrice = overallPrice;
		}

		public string GetClient() => client;
		public DateTime GetDepartureDate() => departureDate;
		public DateTime GetReturnDate() => returnDate;
		public double GetOverallPrice() => overallPrice;

		public override string GetDestination() => destination;
		public override string GetTransport() => prefferedTransport;
		public override string GetLocation() => travelLocation;
		public override int GetLuggageLimit() => luggageLimit;
	}
}
