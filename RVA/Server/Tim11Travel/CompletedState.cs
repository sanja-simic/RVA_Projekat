using System;
using System.Collections.Generic;
using System.Text;
using TravelSystem.Models.Entities;

namespace Tim11.Travel
{
	public class CompletedState : TravelArrangementState
	{
		public override void ChangeState(TravelArrangement arrangement)
		{
			// Kompletiran aranžman ne može da menja stanje
			Console.WriteLine("Travel arrangement is already completed. No state change allowed.");
		}

		public override string GetStateName()
		{
			return "Completed";
		}
	}
}
