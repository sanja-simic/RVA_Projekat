using System;
using System.Collections.Generic;
using System.Text;

namespace Tim_11.Travel
{
	public abstract class TravelArrangementState
	{
		TravelArrangement arrangement;

		public void HandleStateChange()
		{
			throw new NotImplementedException();
		}
	}
}
