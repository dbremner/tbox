using System;
using System.Collections.Generic;
using ScriptEngine.Core.Params;

namespace ScriptEngine.Core.Interfaces
{
	public interface IScriptCompiler : ICompiler
	{
		void Execute(string sourceText, IEnumerable<Parameter> parameters, Action<Action> dispatcher);
		ExecutionContext GetExecutionContext(string sourceText, Action<Action> dispatcher);
	}
}
