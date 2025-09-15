using System;
using TravelSystem.Server.Patterns.Repository;

namespace TravelSystem.Server.Patterns.Factory
{
    /// <summary>
    /// Abstract Factory for creating repositories
    /// </summary>
    public abstract class RepositoryFactory
    {
        /// <summary>
        /// Create repository for specific entity type
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>Repository instance</returns>
        public abstract IGenericRepository<T> CreateRepository<T>() where T : class;

        /// <summary>
        /// Create Unit of Work
        /// </summary>
        /// <returns>Unit of Work instance</returns>
        public abstract IUnitOfWork CreateUnitOfWork();
    }

    /// <summary>
    /// Registry for repository factories
    /// </summary>
    public static class RepositoryFactoryRegistry
    {
        private static RepositoryFactory _defaultFactory;

        /// <summary>
        /// Register default repository factory
        /// </summary>
        /// <param name="factory">Factory instance</param>
        public static void RegisterFactory(RepositoryFactory factory)
        {
            _defaultFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <summary>
        /// Get default repository factory
        /// </summary>
        /// <returns>Factory instance</returns>
        public static RepositoryFactory GetFactory()
        {
            return _defaultFactory ?? throw new InvalidOperationException("No repository factory registered. Call RegisterFactory first.");
        }
    }
}