using System;
using System.Collections.Generic;
using System.Text;

namespace Tim_11.Travel
{
	public class TravelArrangement
	{
		string id;
		private ModeOfTransport asscoiatedTransportation;
		private int numberOfDays;
		public List<Passenger> passengers;
		public Destination destination;
		TravelArrangementState currentState;

		public void SetState(TravelArrangementState state)
		{
			throw new NotImplementedException();
		}

		public void ChangeState()
		{
			throw new NotImplementedException();
		}

		public string GetCurrentStateName()
		{
			throw new NotImplementedException();
		}
	}
}
