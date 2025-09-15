using System;

namespace TravelSystem.Server.Patterns.State
{
    /// <summary>
    /// State manager that provides centralized state management
    /// </summary>
    public class StateManager
    {
        /// <summary>
        /// Execute state transition for a context
        /// </summary>
        /// <param name="context">Context to change state for</param>
        /// <param name="newState">New state to transition to</param>
        public static void TransitionTo(IStateContext context, State newState)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (newState == null) throw new ArgumentNullException(nameof(newState));

            var previousState = context.GetCurrentState();
            context.ChangeState(newState);
            
            Console.WriteLine($"State transition: {previousState?.GetStateName() ?? "None"} -> {newState.GetStateName()}");
        }

        /// <summary>
        /// Handle current state logic
        /// </summary>
        /// <param name="context">Context to handle</param>
        public static void HandleCurrentState(IStateContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            
            var currentState = context.GetCurrentState();
            currentState?.Handle(context);
        }
    }
}