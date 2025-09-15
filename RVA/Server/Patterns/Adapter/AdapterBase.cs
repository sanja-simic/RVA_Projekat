using System;

namespace TravelSystem.Server.Patterns.Adapter
{
    /// <summary>
    /// Abstract base class for adapter implementations
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TTarget">Target type</typeparam>
    public abstract class AdapterBase<TSource, TTarget> : IAdapter<TSource, TTarget>
    {
        /// <summary>
        /// Convert source object to target object
        /// </summary>
        /// <param name="source">Source object</param>
        /// <returns>Adapted target object</returns>
        public virtual TTarget Adapt(TSource source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return DoAdapt(source);
        }

        /// <summary>
        /// Convert target object back to source object
        /// </summary>
        /// <param name="target">Target object</param>
        /// <returns>Adapted source object</returns>
        public virtual TSource AdaptBack(TTarget target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            return DoAdaptBack(target);
        }

        /// <summary>
        /// Implement the actual adaptation logic from source to target
        /// </summary>
        /// <param name="source">Source object</param>
        /// <returns>Adapted target object</returns>
        protected abstract TTarget DoAdapt(TSource source);

        /// <summary>
        /// Implement the actual adaptation logic from target to source
        /// </summary>
        /// <param name="target">Target object</param>
        /// <returns>Adapted source object</returns>
        protected abstract TSource DoAdaptBack(TTarget target);
    }
}