using System;
using System.Collections.Generic;
using System.Text;

namespace Tim11.Travel
{
	public class ReservedState : TravelArrangementState
	{
		public override void ChangeState(TravelArrangement arrangement)
		{
			// Prelazi na PaidState
			arrangement.SetState(new PaidState());
		}

		public override string GetStateName()
		{
			return "Reserved";
		}
	}
}
