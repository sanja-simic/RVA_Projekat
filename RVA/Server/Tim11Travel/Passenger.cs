using System;
using System.Collections.Generic;
using System.Text;

namespace Tim11.Travel
{
	public class Passenger
	{
		public string Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PassportNumber { get; set; }
		public int LuggageWeight { get; set; }

		public Passenger() { }

		public Passenger(string id, string firstName, string lastName, string passportNumber, int luggageWeight)
		{
			Id = id;
			FirstName = firstName;
			LastName = lastName;
			PassportNumber = passportNumber;
			LuggageWeight = luggageWeight;
		}
	}
}
