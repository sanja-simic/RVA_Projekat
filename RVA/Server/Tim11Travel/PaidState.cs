using System;
using System.Collections.Generic;
using System.Text;
using TravelSystem.Models.Entities;

namespace Tim11.Travel
{
	public class PaidState : TravelArrangementState
	{
		public override void ChangeState(TravelArrangement arrangement)
		{
			// Prelazi na InProgressState
			arrangement.ChangeState();
		}

		public override string GetStateName()
		{
			return "Paid";
		}
	}
}
