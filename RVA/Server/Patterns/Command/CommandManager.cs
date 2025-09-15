using System;
using System.Collections.Generic;
using System.Linq;

namespace TravelSystem.Server.Patterns.Command
{
    /// <summary>
    /// Command Manager for executing, undoing and redoing commands
    /// </summary>
    public class CommandManager
    {
        private readonly Stack<ICommand> _undoStack;
        private readonly Stack<ICommand> _redoStack;
        private readonly int _maxHistorySize;

        public CommandManager(int maxHistorySize = 100)
        {
            _maxHistorySize = maxHistorySize;
            _undoStack = new Stack<ICommand>();
            _redoStack = new Stack<ICommand>();
        }

        /// <summary>
        /// Execute a command and add it to history
        /// </summary>
        /// <param name="command">Command to execute</param>
        public void ExecuteCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            try
            {
                command.Execute();
                
                if (command.CanUndo)
                {
                    _undoStack.Push(command);
                    
                    // Limit stack size
                    if (_undoStack.Count > _maxHistorySize)
                    {
                        var commands = _undoStack.ToArray().Reverse().Take(_maxHistorySize).Reverse().ToArray();
                        _undoStack.Clear();
                        foreach (var cmd in commands)
                        {
                            _undoStack.Push(cmd);
                        }
                    }
                }
                
                // Clear redo stack since we're executing a new command
                _redoStack.Clear();
                
                OnCommandExecuted(command);
            }
            catch (Exception ex)
            {
                OnCommandFailed(command, ex);
                throw;
            }
        }

        /// <summary>
        /// Undo the last command
        /// </summary>
        /// <returns>True if undo was successful</returns>
        public bool UndoCommand()
        {
            if (_undoStack.Count == 0) return false;

            var command = _undoStack.Pop();
            try
            {
                command.Undo();
                _redoStack.Push(command);
                OnCommandUndone(command);
                return true;
            }
            catch (Exception ex)
            {
                // Put command back on undo stack if undo failed
                _undoStack.Push(command);
                OnCommandFailed(command, ex);
                throw;
            }
        }

        /// <summary>
        /// Redo the last undone command
        /// </summary>
        /// <returns>True if redo was successful</returns>
        public bool RedoCommand()
        {
            if (_redoStack.Count == 0) return false;

            var command = _redoStack.Pop();
            try
            {
                command.Execute();
                _undoStack.Push(command);
                OnCommandRedone(command);
                return true;
            }
            catch (Exception ex)
            {
                // Put command back on redo stack if redo failed
                _redoStack.Push(command);
                OnCommandFailed(command, ex);
                throw;
            }
        }

        /// <summary>
        /// Check if undo is possible
        /// </summary>
        public bool CanUndo => _undoStack.Count > 0;

        /// <summary>
        /// Check if redo is possible
        /// </summary>
        public bool CanRedo => _redoStack.Count > 0;

        /// <summary>
        /// Get undo history
        /// </summary>
        public IEnumerable<string> GetUndoHistory()
        {
            return _undoStack.Select(cmd => cmd.Description).ToList();
        }

        /// <summary>
        /// Get redo history
        /// </summary>
        public IEnumerable<string> GetRedoHistory()
        {
            return _redoStack.Select(cmd => cmd.Description).ToList();
        }

        /// <summary>
        /// Clear all command history
        /// </summary>
        public void ClearHistory()
        {
            _undoStack.Clear();
            _redoStack.Clear();
        }

        // Events
        public event Action<ICommand> CommandExecuted;
        public event Action<ICommand> CommandUndone;
        public event Action<ICommand> CommandRedone;
        public event Action<ICommand, Exception> CommandFailed;

        protected virtual void OnCommandExecuted(ICommand command) => CommandExecuted?.Invoke(command);
        protected virtual void OnCommandUndone(ICommand command) => CommandUndone?.Invoke(command);
        protected virtual void OnCommandRedone(ICommand command) => CommandRedone?.Invoke(command);
        protected virtual void OnCommandFailed(ICommand command, Exception exception) => CommandFailed?.Invoke(command, exception);
    }
}