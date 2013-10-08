using System;
using System.IO;
using ScriptEngine.Core.Interfaces;

namespace ScriptEngine.Core
{
	public class ScriptContext : SectionContext, IScriptContext
	{
		private readonly string undoPath;

		public ScriptContext(string undoPath)
		{
			this.undoPath = undoPath;
		}

		public string Resolve(string path)
		{
			return path;
		}

		public string GenerateNextUndoPath()
		{
			return Path.Combine(undoPath, Guid.NewGuid().ToString());
		}
	}
}
