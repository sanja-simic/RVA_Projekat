using System;
using System.Collections.Generic;
using System.Text;

namespace Tim_11.Travel
{
	public class CommandManager
	{
		Stack <ICommand> undoStack;
		Stack <ICommand> redoStack;

		public void ExecuteCommand()
		{
			throw new NotImplementedException();
		}
	}
}
