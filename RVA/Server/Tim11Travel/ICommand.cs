using System;
using System.Collections.Generic;
using System.Text;

namespace Tim11.Travel
{
	public interface ICommand
	{
		void Execute();

		void Undo();
	}
}
