namespace TravelSystem.Server.Patterns.State
{
    /// <summary>
    /// Interface for context that uses State pattern
    /// </summary>
    public interface IStateContext
    {
        /// <summary>
        /// Change the current state
        /// </summary>
        /// <param name="state">New state to transition to</param>
        void ChangeState(State state);

        /// <summary>
        /// Get the current state
        /// </summary>
        /// <returns>Current state</returns>
        State GetCurrentState();
    }
}