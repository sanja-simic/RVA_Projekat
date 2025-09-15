using System;
using System.Collections.Generic;

namespace TravelSystem.Server.Patterns.Adapter
{
    /// <summary>
    /// Adapter registry for managing multiple adapters
    /// </summary>
    public class AdapterRegistry
    {
        private static readonly Dictionary<string, object> _adapters = new Dictionary<string, object>();

        /// <summary>
        /// Register an adapter for specific types
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TTarget">Target type</typeparam>
        /// <param name="adapter">Adapter instance</param>
        public static void RegisterAdapter<TSource, TTarget>(IAdapter<TSource, TTarget> adapter)
        {
            if (adapter == null) throw new ArgumentNullException(nameof(adapter));
            
            var key = GetAdapterKey<TSource, TTarget>();
            _adapters[key] = adapter;
        }

        /// <summary>
        /// Get an adapter for specific types
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TTarget">Target type</typeparam>
        /// <returns>Adapter instance</returns>
        public static IAdapter<TSource, TTarget> GetAdapter<TSource, TTarget>()
        {
            var key = GetAdapterKey<TSource, TTarget>();
            if (_adapters.TryGetValue(key, out var adapter))
            {
                return (IAdapter<TSource, TTarget>)adapter;
            }
            throw new InvalidOperationException($"No adapter registered for {typeof(TSource).Name} to {typeof(TTarget).Name}");
        }

        /// <summary>
        /// Check if adapter is registered for specific types
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TTarget">Target type</typeparam>
        /// <returns>True if adapter is registered</returns>
        public static bool HasAdapter<TSource, TTarget>()
        {
            var key = GetAdapterKey<TSource, TTarget>();
            return _adapters.ContainsKey(key);
        }

        private static string GetAdapterKey<TSource, TTarget>()
        {
            return $"{typeof(TSource).FullName}->{typeof(TTarget).FullName}";
        }
    }
}