using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TravelSystem.Server.Patterns.Repository
{
    /// <summary>
    /// Abstract base implementation of Generic Repository pattern
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public abstract class GenericRepositoryBase<T> : IGenericRepository<T> where T : class
    {
        protected readonly List<T> _entities;

        protected GenericRepositoryBase()
        {
            _entities = new List<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _entities.ToList();
        }

        public virtual T GetById(object id)
        {
            return _entities.FirstOrDefault(e => GetEntityId(e).Equals(id));
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _entities.AsQueryable().Where(predicate).ToList();
        }

        public virtual void Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _entities.Add(entity);
            OnEntityAdded(entity);
        }

        public virtual void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            
            var existingEntity = GetById(GetEntityId(entity));
            if (existingEntity != null)
            {
                var index = _entities.IndexOf(existingEntity);
                _entities[index] = entity;
                OnEntityUpdated(entity);
            }
        }

        public virtual void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            
            if (_entities.Remove(entity))
            {
                OnEntityDeleted(entity);
            }
        }

        public virtual void DeleteById(object id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public virtual int Count()
        {
            return _entities.Count;
        }

        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            return _entities.AsQueryable().Any(predicate);
        }

        /// <summary>
        /// Extract entity ID - must be implemented by derived classes
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity ID</returns>
        protected abstract object GetEntityId(T entity);

        /// <summary>
        /// Called when entity is added
        /// </summary>
        /// <param name="entity">Added entity</param>
        protected virtual void OnEntityAdded(T entity) { }

        /// <summary>
        /// Called when entity is updated
        /// </summary>
        /// <param name="entity">Updated entity</param>
        protected virtual void OnEntityUpdated(T entity) { }

        /// <summary>
        /// Called when entity is deleted
        /// </summary>
        /// <param name="entity">Deleted entity</param>
        protected virtual void OnEntityDeleted(T entity) { }
    }
}