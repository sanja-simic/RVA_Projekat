using System;
using System.Collections.Generic;
using System.Linq;

namespace TravelSystem.Server.Patterns.Command
{
    /// <summary>
    /// Composite command that executes multiple commands as a single unit
    /// </summary>
    public class CompositeCommand : CommandBase
    {
        private readonly List<ICommand> _commands;
        private readonly List<ICommand> _executedCommands; // Track successfully executed commands
        private bool _isExecuted = false;

        public CompositeCommand(string description) : base(description)
        {
            _commands = new List<ICommand>();
            _executedCommands = new List<ICommand>();
        }

        /// <summary>
        /// Add a command to the composite
        /// </summary>
        /// <param name="command">Command to add</param>
        public void AddCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (_isExecuted) throw new InvalidOperationException("Cannot add commands after execution");
            _commands.Add(command);
        }

        /// <summary>
        /// Execute all commands in order
        /// </summary>
        public override void Execute()
        {
            if (_isExecuted) throw new InvalidOperationException("Command has already been executed");

            _executedCommands.Clear();

            try
            {
                foreach (var command in _commands)
                {
                    command.Execute();
                    _executedCommands.Add(command);
                }

                _isExecuted = true;
                MarkAsExecuted();
            }
            catch (Exception)
            {
                // Rollback executed commands in reverse order
                RollbackExecutedCommands();
                throw;
            }
        }

        /// <summary>
        /// Undo all successfully executed commands in reverse order
        /// </summary>
        public override void Undo()
        {
            if (!_isExecuted) throw new InvalidOperationException("Command has not been executed yet");

            var undoErrors = new List<Exception>();

            // Undo only the commands that were successfully executed, in reverse order
            for (int i = _executedCommands.Count - 1; i >= 0; i--)
            {
                try
                {
                    if (_executedCommands[i].CanUndo)
                    {
                        _executedCommands[i].Undo();
                    }
                }
                catch (Exception ex)
                {
                    undoErrors.Add(ex);
                }
            }

            _isExecuted = false;

            // If there were undo errors, throw an aggregate exception
            if (undoErrors.Count > 0)
            {
                throw new AggregateException("One or more commands failed to undo", undoErrors);
            }
        }

        /// <summary>
        /// Can undo only if executed and all executed commands can be undone
        /// </summary>
        public override bool CanUndo => _isExecuted && _executedCommands.All(cmd => cmd.CanUndo);

        /// <summary>
        /// Get number of commands in composite
        /// </summary>
        public int CommandCount => _commands.Count;

        /// <summary>
        /// Get number of successfully executed commands
        /// </summary>
        public int ExecutedCommandCount => _executedCommands.Count;

        /// <summary>
        /// Check if the composite has been executed
        /// </summary>
        public bool IsExecuted => _isExecuted;

        /// <summary>
        /// Private method to rollback executed commands during a failed execution
        /// </summary>
        private void RollbackExecutedCommands()
        {
            for (int i = _executedCommands.Count - 1; i >= 0; i--)
            {
                try
                {
                    if (_executedCommands[i].CanUndo)
                    {
                        _executedCommands[i].Undo();
                    }
                }
                catch
                {
                    // Log but don't throw - we're already in error state
                    // In a real application, you'd want to log this properly
                }
            }
            _executedCommands.Clear();
        }

        /// <summary>
        /// Clear all commands (only allowed if not executed)
        /// </summary>
        public void Clear()
        {
            if (_isExecuted) throw new InvalidOperationException("Cannot clear commands after execution");
            _commands.Clear();
        }

        /// <summary>
        /// Get descriptions of all commands
        /// </summary>
        public IEnumerable<string> GetCommandDescriptions()
        {
            return _commands.Select(cmd => cmd.Description);
        }

        /// <summary>
        /// Get descriptions of successfully executed commands
        /// </summary>
        public IEnumerable<string> GetExecutedCommandDescriptions()
        {
            return _executedCommands.Select(cmd => cmd.Description);
        }
    }
}