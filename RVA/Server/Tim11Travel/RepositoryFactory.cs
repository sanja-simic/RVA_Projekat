using System;
using System.Collections.Generic;
using System.Text;

namespace Tim11.Travel
{
	public class RepositoryFactory
	{
		public static ITravelArrangementRepository CreateRepository(string format)
		{
			switch (format.ToUpper())
			{
				case "CSV":
					return new CSVRepository();
				case "JSON":
					return new JSONRepository();
				case "XML":
					return new XMLRepository();
				default:
					throw new ArgumentException($"Unsupported repository format: {format}");
			}
		}
	}
}
