using System;
using System.Collections.Generic;
using ScriptEngine.Core;
using ScriptEngine.Core.Params;

namespace Automator.Code
{
	interface IRunner
	{
		void Build(string text);
		void Run(string path, string rootPath, ICollection<Parameter> parameters, Action<Action> dispatcher);
		ExecutionContext GetExecutionContext(string path, Action<Action> dispatcher);
	}
}
