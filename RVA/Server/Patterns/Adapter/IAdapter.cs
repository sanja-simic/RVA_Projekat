namespace TravelSystem.Server.Patterns.Adapter
{
    /// <summary>
    /// Generic Adapter interface for adapting between two types
    /// </summary>
    /// <typeparam name="TSource">Source type to adapt from</typeparam>
    /// <typeparam name="TTarget">Target type to adapt to</typeparam>
    public interface IAdapter<TSource, TTarget>
    {
        /// <summary>
        /// Convert source object to target object
        /// </summary>
        /// <param name="source">Source object</param>
        /// <returns>Adapted target object</returns>
        TTarget Adapt(TSource source);

        /// <summary>
        /// Convert target object back to source object
        /// </summary>
        /// <param name="target">Target object</param>
        /// <returns>Adapted source object</returns>
        TSource AdaptBack(TTarget target);
    }
}