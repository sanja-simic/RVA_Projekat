using System;
using System.Collections.Generic;
using System.Linq;
using TravelSystem.Models.DTOs;
using TravelSystem.Models.Enums;

namespace TravelSystem.Client.Helpers
{
    /// <summary>
    /// Helper class for simulating state transitions of travel arrangements
    /// </summary>
    public static class StateTransitionHelper
    {
        private static readonly Dictionary<EntityState, List<EntityState>> ValidTransitions = 
            new Dictionary<EntityState, List<EntityState>>
            {
                { EntityState.Reserved, new List<EntityState> { EntityState.Paid, EntityState.InProgress } },
                { EntityState.Paid, new List<EntityState> { EntityState.InProgress } },
                { EntityState.InProgress, new List<EntityState> { EntityState.Completed } },
                { EntityState.Completed, new List<EntityState>() } // No transitions from completed
            };

        /// <summary>
        /// Gets valid next states for a given current state
        /// </summary>
        public static IEnumerable<EntityState> GetValidNextStates(EntityState currentState)
        {
            return ValidTransitions.ContainsKey(currentState) 
                ? ValidTransitions[currentState] 
                : new List<EntityState>();
        }

        /// <summary>
        /// Checks if transition from one state to another is valid
        /// </summary>
        public static bool IsValidTransition(EntityState fromState, EntityState toState)
        {
            return GetValidNextStates(fromState).Contains(toState);
        }

        /// <summary>
        /// Gets all possible state transitions for an arrangement
        /// </summary>
        public static List<StateTransition> GetAllPossibleTransitions(TravelArrangementDto arrangement)
        {
            var transitions = new List<StateTransition>();
            var currentState = arrangement.State;

            // Add transition through all valid states
            AddTransitionsRecursively(arrangement.Id, currentState, transitions, new HashSet<EntityState>());

            return transitions;
        }

        private static void AddTransitionsRecursively(
            string arrangementId, 
            EntityState currentState, 
            List<StateTransition> transitions, 
            HashSet<EntityState> visitedStates)
        {
            if (visitedStates.Contains(currentState))
                return;

            visitedStates.Add(currentState);

            var nextStates = GetValidNextStates(currentState);
            foreach (var nextState in nextStates)
            {
                transitions.Add(new StateTransition
                {
                    ArrangementId = arrangementId,
                    FromState = currentState,
                    ToState = nextState,
                    TransitionTime = DateTime.Now.AddMinutes(transitions.Count * 2) // Simulate time progression
                });

                AddTransitionsRecursively(arrangementId, nextState, transitions, visitedStates);
            }
        }

        /// <summary>
        /// Simulates automatic state progression for an arrangement
        /// </summary>
        public static EntityState SimulateNextState(EntityState currentState)
        {
            var nextStates = GetValidNextStates(currentState);
            if (nextStates.Any())
            {
                // For simulation, just take the first valid next state
                return nextStates.First();
            }
            return currentState;
        }
    }

    /// <summary>
    /// Represents a state transition
    /// </summary>
    public class StateTransition
    {
        public string ArrangementId { get; set; }
        public EntityState FromState { get; set; }
        public EntityState ToState { get; set; }
        public DateTime TransitionTime { get; set; }
        public string Description => $"{FromState} → {ToState}";
    }
}