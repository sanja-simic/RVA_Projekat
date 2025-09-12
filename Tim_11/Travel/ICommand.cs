using System;
using System.Collections.Generic;
using System.Text;

namespace Tim_11.Travel
{
	public interface ICommand
	{
		void Execute();

		void Undo();
	}
}
