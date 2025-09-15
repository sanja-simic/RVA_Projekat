using System;

namespace TravelSystem.Server.Patterns.State
{
    /// <summary>
    /// Abstract State class for State pattern implementation
    /// </summary>
    public abstract class State
    {
        /// <summary>
        /// Handle state transition
        /// </summary>
        /// <param name="context">Context object that uses this state</param>
        public abstract void Handle(IStateContext context);

        /// <summary>
        /// Get the name of this state
        /// </summary>
        /// <returns>State name</returns>
        public abstract string GetStateName();
    }
}