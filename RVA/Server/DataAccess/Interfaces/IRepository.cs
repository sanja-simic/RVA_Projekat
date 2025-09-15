using System.Collections.Generic;
using TravelSystem.Models.Entities;

namespace TravelSystem.Server.DataAccess.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        List<T> GetAll();
        T GetById(string id);
        void Add(T entity);
        void Update(T entity);
        void Delete(string id);
        bool Exists(string id);
    }
}
