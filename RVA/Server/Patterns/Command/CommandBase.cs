using System;

namespace TravelSystem.Server.Patterns.Command
{
    /// <summary>
    /// Abstract base class for Command pattern implementations
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        protected CommandBase(string description)
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Undo the command
        /// </summary>
        public abstract void Undo();

        /// <summary>
        /// Indicates if this command can be undone
        /// </summary>
        public virtual bool CanUndo => true;

        /// <summary>
        /// Description of what this command does
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        /// Timestamp when command was created
        /// </summary>
        public DateTime CreatedAt { get; } = DateTime.Now;

        /// <summary>
        /// Timestamp when command was executed
        /// </summary>
        public DateTime? ExecutedAt { get; protected set; }

        /// <summary>
        /// Mark command as executed
        /// </summary>
        protected void MarkAsExecuted()
        {
            ExecutedAt = DateTime.Now;
        }
    }
}