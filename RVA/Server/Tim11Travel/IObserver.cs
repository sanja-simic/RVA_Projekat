using System;
using System.Collections.Generic;
using System.Text;

namespace Tim11.Travel
{
	public interface IObserver
	{
		void Update(TravelArrangement arrangement, string eventType);
	}
}
