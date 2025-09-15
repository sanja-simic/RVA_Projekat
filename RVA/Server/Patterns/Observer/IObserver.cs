using TravelSystem.Models.Entities;

namespace TravelSystem.Server.Patterns.Observer
{
    public interface IObserver<T> where T : BaseEntity
    {
        void Update(T entity, string eventType);
    }
}
