using System;
using System.Collections.Generic;
using System.Text;

namespace Tim11.Travel
{
	public class TravelArrangementSubject
	{
		private List<IObserver> observers;

		public TravelArrangementSubject()
		{
			observers = new List<IObserver>();
		}

		public void RegisterObserver(IObserver observer)
		{
			if (!observers.Contains(observer))
			{
				observers.Add(observer);
			}
		}

		public void UnregisterObserver(IObserver observer)
		{
			observers.Remove(observer);
		}

		public void NotifyObservers(TravelArrangement arrangement, string eventType)
		{
			foreach (var observer in observers)
			{
				observer.Update(arrangement, eventType);
			}
		}
	}
}
