using System;
using System.Collections.Generic;
using System.Text;

namespace Tim11.Travel
{
	public class PaidState : TravelArrangementState
	{
		public override void ChangeState(TravelArrangement arrangement)
		{
			// Prelazi na InProgressState
			arrangement.SetState(new InProgressState());
		}

		public override string GetStateName()
		{
			return "Paid";
		}
	}
}
