using System;
using System.Collections.Generic;
using System.Text;
using TravelSystem.Models.Entities;
using TravelSystem.Server.Patterns.Observer;

namespace Tim11.Travel
{
	public class TravelService
	{
		private ITravelArrangementRepository repository;
		private TravelArrangementSubject subject;

		public TravelService(ITravelArrangementRepository repository)
		{
			this.repository = repository;
			this.subject = new TravelArrangementSubject();
		}

		public void RegisterObserver(TravelSystem.Server.Patterns.Observer.IObserver<TravelArrangement> observer)
		{
			subject.RegisterObserver(observer);
		}

		public void UnregisterObserver(TravelSystem.Server.Patterns.Observer.IObserver<TravelArrangement> observer)
		{
			subject.UnregisterObserver(observer);
		}

		public List<TravelArrangement> GetAllArrangements()
		{
			return repository.GetAll();
		}

		public void AddArrangement(TravelArrangement arrangement)
		{
			repository.Add(arrangement);
			subject.NotifyObservers(arrangement, "ADDED");
		}

		public void UpdateArrangement(TravelArrangement arrangement)
		{
			repository.Update(arrangement);
			subject.NotifyObservers(arrangement, "UPDATED");
		}

		public void DeleteArrangement(string id)
		{
			var arrangements = repository.GetAll();
			var arrangement = arrangements.Find(a => a.Id == id);
			if (arrangement != null)
			{
				repository.Delete(id);
				subject.NotifyObservers(arrangement, "DELETED");
			}
		}
	}
}
