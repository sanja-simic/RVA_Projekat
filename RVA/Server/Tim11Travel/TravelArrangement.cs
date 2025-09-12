using System;
using System.Collections.Generic;
using System.Text;

namespace Tim11.Travel
{
	public class TravelArrangement
	{
		public string Id { get; set; }
		public ModeOfTransport AssociatedTransportation { get; set; }
		public int NumberOfDays { get; set; }
		public List<Passenger> Passengers { get; set; }
		public Destination Destination { get; set; }
		public TravelArrangementState CurrentState { get; set; }

		public TravelArrangement()
		{
			Passengers = new List<Passenger>();
			CurrentState = new ReservedState();
		}

		public TravelArrangement(string id, ModeOfTransport transportation, int numberOfDays, Destination destination)
		{
			Id = id;
			AssociatedTransportation = transportation;
			NumberOfDays = numberOfDays;
			Destination = destination;
			Passengers = new List<Passenger>();
			CurrentState = new ReservedState();
		}

		public void SetState(TravelArrangementState state)
		{
			CurrentState = state;
		}

		public void ChangeState()
		{
			CurrentState?.ChangeState(this);
		}

		public string GetCurrentStateName()
		{
			return CurrentState?.GetType().Name ?? "Unknown";
		}
	}
}
