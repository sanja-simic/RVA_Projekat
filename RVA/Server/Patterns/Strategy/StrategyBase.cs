using System;

namespace TravelSystem.Server.Patterns.Strategy
{
    /// <summary>
    /// Base class for Strategy implementations
    /// </summary>
    /// <typeparam name="TInput">Input type for the strategy</typeparam>
    /// <typeparam name="TOutput">Output type for the strategy</typeparam>
    public abstract class StrategyBase<TInput, TOutput> : IStrategy<TInput, TOutput>
    {
        public abstract string Name { get; }
        public abstract string Description { get; }

        public abstract TOutput Execute(TInput input);

        protected virtual void ValidateInput(TInput input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input), "Strategy input cannot be null");
        }

        protected virtual void OnExecutionStarted(TInput input)
        {
            // Override in derived classes if needed
        }

        protected virtual void OnExecutionCompleted(TInput input, TOutput output)
        {
            // Override in derived classes if needed
        }

        protected virtual void OnExecutionFailed(TInput input, Exception exception)
        {
            // Override in derived classes if needed
        }
    }
}