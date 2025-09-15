using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TravelSystem.Server.Patterns.Repository
{
    /// <summary>
    /// Generic Repository pattern interface
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns>Collection of entities</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Get entity by ID
        /// </summary>
        /// <param name="id">Entity identifier</param>
        /// <returns>Entity or null if not found</returns>
        T GetById(object id);

        /// <summary>
        /// Find entities by predicate
        /// </summary>
        /// <param name="predicate">Search predicate</param>
        /// <returns>Collection of matching entities</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Add new entity
        /// </summary>
        /// <param name="entity">Entity to add</param>
        void Add(T entity);

        /// <summary>
        /// Update existing entity
        /// </summary>
        /// <param name="entity">Entity to update</param>
        void Update(T entity);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        void Delete(T entity);

        /// <summary>
        /// Delete entity by ID
        /// </summary>
        /// <param name="id">Entity identifier</param>
        void DeleteById(object id);

        /// <summary>
        /// Get count of entities
        /// </summary>
        /// <returns>Total count</returns>
        int Count();

        /// <summary>
        /// Check if entity exists
        /// </summary>
        /// <param name="predicate">Search predicate</param>
        /// <returns>True if exists</returns>
        bool Exists(Expression<Func<T, bool>> predicate);
    }
}