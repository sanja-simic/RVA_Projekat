using System;

namespace TravelSystem.Server.Patterns.Factory
{
    /// <summary>
    /// Concrete Repository Factory for Travel domain
    /// </summary>
    public class TravelRepositoryFactory
    {
        /// <summary>
        /// Create repository based on format type
        /// </summary>
        /// <param name="format">Repository format (CSV, JSON, XML)</param>
        /// <returns>Repository instance</returns>
        public static object CreateRepository(string format)
        {
            if (string.IsNullOrEmpty(format))
                throw new ArgumentNullException(nameof(format));

            switch (format.ToUpper())
            {
                case "CSV":
                    return CreateCSVRepository();
                case "JSON":
                    return CreateJSONRepository();
                case "XML":
                    return CreateXMLRepository();
                default:
                    throw new ArgumentException($"Unsupported repository format: {format}");
            }
        }

        /// <summary>
        /// Create repository with specific type
        /// </summary>
        /// <typeparam name="T">Repository type</typeparam>
        /// <param name="format">Format type</param>
        /// <returns>Typed repository</returns>
        public static T CreateRepository<T>(string format) where T : class
        {
            var repository = CreateRepository(format);
            return repository as T ?? throw new InvalidCastException($"Cannot cast repository to type {typeof(T).Name}");
        }

        private static object CreateCSVRepository()
        {
            // This will be resolved at runtime with proper implementation
            return Activator.CreateInstance("Tim11.Travel", "Tim11.Travel.CSVRepository");
        }

        private static object CreateJSONRepository()
        {
            // This will be resolved at runtime with proper implementation
            return Activator.CreateInstance("Tim11.Travel", "Tim11.Travel.JSONRepository");
        }

        private static object CreateXMLRepository()
        {
            // This will be resolved at runtime with proper implementation
            return Activator.CreateInstance("Tim11.Travel", "Tim11.Travel.XMLRepository");
        }
    }
}