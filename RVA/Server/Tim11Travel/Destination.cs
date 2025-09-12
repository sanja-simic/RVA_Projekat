using System;
using System.Collections.Generic;
using System.Text;

namespace Tim11.Travel
{
	public class Destination
	{
		public string Id { get; set; }
		public string TownName { get; set; }
		public string CountryName { get; set; }
		public double StayPriceByDay { get; set; }

		public Destination() { }

		public Destination(string id, string townName, string countryName, double stayPriceByDay)
		{
			Id = id;
			TownName = townName;
			CountryName = countryName;
			StayPriceByDay = stayPriceByDay;
		}
	}
}
