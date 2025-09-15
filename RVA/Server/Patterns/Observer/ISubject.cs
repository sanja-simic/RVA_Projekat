using System.Collections.Generic;
using TravelSystem.Models.Entities;

namespace TravelSystem.Server.Patterns.Observer
{
    public interface ISubject<T> where T : BaseEntity
    {
        void RegisterObserver(IObserver<T> observer);
        void UnregisterObserver(IObserver<T> observer);
        void NotifyObservers(T entity, string eventType);
    }
}
