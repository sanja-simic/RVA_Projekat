using System;
using System.Collections.Generic;
using System.Text;
using TravelSystem.Models.Entities;

namespace Tim11.Travel
{
	public class InProgressState : TravelArrangementState
	{
		public override void ChangeState(TravelArrangement arrangement)
		{
			// Prelazi na CompletedState
			arrangement.ChangeState();
		}

		public override string GetStateName()
		{
			return "InProgress";
		}
	}
}
