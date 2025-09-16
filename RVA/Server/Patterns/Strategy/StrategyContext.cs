using System;
using System.Collections.Generic;
using System.Linq;

namespace TravelSystem.Server.Patterns.Strategy
{
    /// <summary>
    /// Context class that uses Strategy pattern
    /// </summary>
    /// <typeparam name="TInput">Input type for strategies</typeparam>
    /// <typeparam name="TOutput">Output type for strategies</typeparam>
    public class StrategyContext<TInput, TOutput>
    {
        private IStrategy<TInput, TOutput> _strategy;
        private readonly Dictionary<string, IStrategy<TInput, TOutput>> _strategies;

        public StrategyContext()
        {
            _strategies = new Dictionary<string, IStrategy<TInput, TOutput>>();
        }

        public StrategyContext(IStrategy<TInput, TOutput> strategy) : this()
        {
            SetStrategy(strategy);
        }

        /// <summary>
        /// Set the current strategy
        /// </summary>
        /// <param name="strategy">Strategy to use</param>
        public void SetStrategy(IStrategy<TInput, TOutput> strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        /// <summary>
        /// Register a strategy with a name
        /// </summary>
        /// <param name="name">Strategy name</param>
        /// <param name="strategy">Strategy instance</param>
        public void RegisterStrategy(string name, IStrategy<TInput, TOutput> strategy)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Strategy name cannot be null or empty", nameof(name));
            
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            _strategies[name] = strategy;
        }

        /// <summary>
        /// Set strategy by name
        /// </summary>
        /// <param name="name">Strategy name</param>
        public void SetStrategy(string name)
        {
            if (!_strategies.TryGetValue(name, out var strategy))
                throw new ArgumentException($"Strategy '{name}' not found", nameof(name));

            SetStrategy(strategy);
        }

        /// <summary>
        /// Execute the current strategy
        /// </summary>
        /// <param name="input">Input data</param>
        /// <returns>Strategy result</returns>
        public TOutput ExecuteStrategy(TInput input)
        {
            if (_strategy == null)
                throw new InvalidOperationException("No strategy set");

            return _strategy.Execute(input);
        }

        /// <summary>
        /// Get current strategy name
        /// </summary>
        public string CurrentStrategyName => _strategy?.Name;

        /// <summary>
        /// Get all registered strategy names
        /// </summary>
        public IEnumerable<string> GetStrategyNames()
        {
            return _strategies.Keys.ToList();
        }

        /// <summary>
        /// Get strategy by name
        /// </summary>
        /// <param name="name">Strategy name</param>
        /// <returns>Strategy instance or null</returns>
        public IStrategy<TInput, TOutput> GetStrategy(string name)
        {
            _strategies.TryGetValue(name, out var strategy);
            return strategy;
        }
    }
}