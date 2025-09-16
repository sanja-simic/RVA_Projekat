using System;
using System.Collections.Generic;
using System.Text;
using TravelSystem.Server.Patterns.Observer;
using TravelSystem.Models.Entities;

namespace Tim11.Travel
{
	public class TravelArrangementSubject : ISubject<TravelArrangement>
	{
		private List<TravelSystem.Server.Patterns.Observer.IObserver<TravelArrangement>> observers;

		public TravelArrangementSubject()
		{
			observers = new List<TravelSystem.Server.Patterns.Observer.IObserver<TravelArrangement>>();
		}

		public void RegisterObserver(TravelSystem.Server.Patterns.Observer.IObserver<TravelArrangement> observer)
		{
			if (!observers.Contains(observer))
			{
				observers.Add(observer);
			}
		}

		public void UnregisterObserver(TravelSystem.Server.Patterns.Observer.IObserver<TravelArrangement> observer)
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
