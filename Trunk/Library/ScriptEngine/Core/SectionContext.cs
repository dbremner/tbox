using System.Collections.Generic;
using ScriptEngine.Core.Interfaces;

namespace ScriptEngine.Core
{
	public class SectionContext : ISectionContext
	{
		private readonly IList<string> undo = new List<string>();

		public void AddUndo(string function, params object[] args)
		{
			undo.Add(string.Format("{0}({1});", function, string.Join(", ", args)));
		}

		public bool UndoEmpty { get { return undo.Count == 0; } }
	}
}
