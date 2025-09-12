using System;
using System.Collections.Generic;
using System.Text;

namespace Tim11.Travel
{
	public class CommandManager
	{
		private Stack<ICommand> undoStack;
		private Stack<ICommand> redoStack;

		public CommandManager()
		{
			undoStack = new Stack<ICommand>();
			redoStack = new Stack<ICommand>();
		}

		public void ExecuteCommand(ICommand command)
		{
			command.Execute();
			undoStack.Push(command);
			redoStack.Clear(); // Briše redo stack kada se izvršava nova komanda
		}

		public void UndoCommand()
		{
			if (undoStack.Count > 0)
			{
				var command = undoStack.Pop();
				command.Undo();
				redoStack.Push(command);
			}
		}

		public void RedoCommand()
		{
			if (redoStack.Count > 0)
			{
				var command = redoStack.Pop();
				command.Execute();
				undoStack.Push(command);
			}
		}
	}
}
