using System;
using System.Collections.Generic;

namespace TravelSystem.Server.Patterns.Command
{
    /// <summary>
    /// Composite command that executes multiple commands as a single unit
    /// </summary>
    public class CompositeCommand : CommandBase
    {
        private readonly List<ICommand> _commands;

        public CompositeCommand(string description) : base(description)
        {
            _commands = new List<ICommand>();
        }

        /// <summary>
        /// Add a command to the composite
        /// </summary>
        /// <param name="command">Command to add</param>
        public void AddCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            _commands.Add(command);
        }

        /// <summary>
        /// Execute all commands in order
        /// </summary>
        public override void Execute()
        {
            var executedCommands = new List<ICommand>();
            
            try
            {
                foreach (var command in _commands)
                {
                    command.Execute();
                    executedCommands.Add(command);
                }
                MarkAsExecuted();
            }
            catch (Exception)
            {
                // Rollback executed commands in reverse order
                for (int i = executedCommands.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        executedCommands[i].Undo();
                    }
                    catch
                    {
                        // Log but don't throw - we're already in error state
                    }
                }
                throw;
            }
        }

        /// <summary>
        /// Undo all commands in reverse order
        /// </summary>
        public override void Undo()
        {
            for (int i = _commands.Count - 1; i >= 0; i--)
            {
                _commands[i].Undo();
            }
        }

        /// <summary>
        /// Can undo only if all commands can be undone
        /// </summary>
        public override bool CanUndo => _commands.TrueForAll(cmd => cmd.CanUndo);

        /// <summary>
        /// Get number of commands in composite
        /// </summary>
        public int CommandCount => _commands.Count;
    }
}