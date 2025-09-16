using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace TravelSystem.Client.Commands
{
    /// <summary>
    /// Interface for undoable commands
    /// </summary>
    public interface IUndoableCommand : ICommand
    {
        void Execute();
        void Undo();
        string Description { get; }
    }

    /// <summary>
    /// Command manager for handling undo/redo operations
    /// </summary>
    public class UndoRedoManager
    {
        private readonly Stack<IUndoableCommand> _undoStack;
        private readonly Stack<IUndoableCommand> _redoStack;

        public UndoRedoManager()
        {
            _undoStack = new Stack<IUndoableCommand>();
            _redoStack = new Stack<IUndoableCommand>();
        }

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        public event EventHandler CanUndoRedoChanged;

        public void ExecuteCommand(IUndoableCommand command)
        {
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear(); // Clear redo stack when new command is executed
            OnCanUndoRedoChanged();
        }

        public void Undo()
        {
            if (!CanUndo)
                return;

            var command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);
            OnCanUndoRedoChanged();
        }

        public void Redo()
        {
            if (!CanRedo)
                return;

            var command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);
            OnCanUndoRedoChanged();
        }

        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
            OnCanUndoRedoChanged();
        }

        private void OnCanUndoRedoChanged()
        {
            CanUndoRedoChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Base class for undoable commands
    /// </summary>
    public abstract class UndoableCommand : IUndoableCommand
    {
        public abstract string Description { get; }

        public event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Execute();
        }

        public abstract void Execute();
        public abstract void Undo();

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}