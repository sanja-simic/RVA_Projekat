using System;
using System.Collections.Generic;
using System.Text;

namespace Tim11.Travel
{
	public abstract class AgencyTravel
	{
		public string Id { get; set; }
		protected string destination;
		protected string prefferedTransport;
		protected string travelLocation;
		protected int luggageLimit;

		protected AgencyTravel(string id, string destination, string prefferedTransport, string travelLocation, int luggageLimit)
		{
			Id = id;
			this.destination = destination;
			this.prefferedTransport = prefferedTransport;
			this.travelLocation = travelLocation;
			this.luggageLimit = luggageLimit;
		}

		public abstract string GetDestination();
		public abstract string GetTransport();
		public abstract string GetLocation();
		public abstract int GetLuggageLimit();
	}
}
