using System;
using System.Collections.Generic;
using System.Text;
using TravelSystem.Models.Entities;

namespace Tim11.Travel
{
	public abstract class TravelArrangementState
	{
		public abstract void ChangeState(TravelArrangement arrangement);
		public abstract string GetStateName();
	}
}
