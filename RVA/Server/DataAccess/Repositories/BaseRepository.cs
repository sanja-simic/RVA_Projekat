using System;
using System.Collections.Generic;
using System.Linq;
using TravelSystem.Models.Entities;
using TravelSystem.Server.DataAccess.Interfaces;

namespace TravelSystem.Server.DataAccess.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected List<T> _entities;

        protected BaseRepository()
        {
            _entities = new List<T>();
        }

        public virtual List<T> GetAll()
        {
            return _entities.ToList();
        }

        public virtual T GetById(string id)
        {
            return _entities.FirstOrDefault(e => e.Id == id);
        }

        public virtual void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (Exists(entity.Id))
                throw new InvalidOperationException($"Entity with ID {entity.Id} already exists");

            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            _entities.Add(entity);
        }

        public virtual void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var existingEntity = GetById(entity.Id);
            if (existingEntity == null)
                throw new InvalidOperationException($"Entity with ID {entity.Id} not found");

            entity.UpdatedAt = DateTime.Now;
            var index = _entities.IndexOf(existingEntity);
            _entities[index] = entity;
        }

        public virtual void Delete(string id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _entities.Remove(entity);
            }
        }

        public virtual bool Exists(string id)
        {
            return _entities.Any(e => e.Id == id);
        }
    }
}
