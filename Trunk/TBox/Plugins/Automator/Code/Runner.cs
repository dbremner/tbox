using System;
using System.Collections.Generic;
using System.IO;
using ScriptEngine.Core;
using ScriptEngine.Core.Interfaces;
using ScriptEngine.Core.Params;

namespace Automator.Code
{
	sealed class Runner : IRunner, IDisposable
	{
		private readonly ICollection<Parameter> parameters;
		private readonly IScriptCompiler compiler;
		public Runner(string rootPath, ICollection<Parameter> parameters)
		{
			this.parameters = parameters;
			var undoPath = Path.Combine(rootPath, "Undo");
			var context = new ScriptContext(undoPath);
			compiler = new ScriptCompiler(context);
		}

		public void Dispose()
		{
			
		}

		public void Run(string path, Action<Action> dispatcher)
		{
			compiler.Execute(File.ReadAllText(path), parameters, dispatcher);
		}
	}
}
