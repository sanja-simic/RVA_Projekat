namespace TravelSystem.Server.Patterns.Strategy
{
    /// <summary>
    /// Strategy interface for defining various algorithms
    /// </summary>
    /// <typeparam name="TInput">Input type for the strategy</typeparam>
    /// <typeparam name="TOutput">Output type for the strategy</typeparam>
    public interface IStrategy<TInput, TOutput>
    {
        /// <summary>
        /// Execute the strategy algorithm
        /// </summary>
        /// <param name="input">Input data</param>
        /// <returns>Result of the strategy execution</returns>
        TOutput Execute(TInput input);
        
        /// <summary>
        /// Strategy name for identification
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Strategy description
        /// </summary>
        string Description { get; }
    }
}