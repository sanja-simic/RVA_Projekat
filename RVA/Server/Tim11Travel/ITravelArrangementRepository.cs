using System;
using System.Collections.Generic;
using System.Text;
using TravelSystem.Models.Entities;

namespace Tim11.Travel
{
	public interface ITravelArrangementRepository
	{
		List<TravelArrangement> GetAll();

		void Add(TravelArrangement arrangement);

		void Update(TravelArrangement arrangement);

		void Delete(string id);
	}
}