using System;

namespace TravelSystem.Server.Patterns.Repository
{
    /// <summary>
    /// Unit of Work pattern interface for managing transactions
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Get repository for specific entity type
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>Repository instance</returns>
        IGenericRepository<T> GetRepository<T>() where T : class;

        /// <summary>
        /// Save all changes
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Begin transaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Commit transaction
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rollback transaction
        /// </summary>
        void RollbackTransaction();
    }
}