using System;
using System.Collections.Generic;
using System.Text;

namespace Tim11.Travel
{
	public class InProgressState : TravelArrangementState
	{
		public override void ChangeState(TravelArrangement arrangement)
		{
			// Prelazi na CompletedState
			arrangement.SetState(new CompletedState());
		}

		public override string GetStateName()
		{
			return "InProgress";
		}
	}
}
