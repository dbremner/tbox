using System;
using System.Collections.Generic;
using ScriptEngine.Core.Params;

namespace ScriptEngine.Core
{
	public class ExecutionContext
	{
		private readonly Action<IList<Parameter>> executor;
		public IList<Parameter> Parameters { get; private set; }

		internal ExecutionContext(Action<IList<Parameter>> executor, IList<Parameter> parameters)
		{
			this.executor = executor;
			Parameters = parameters;
		}

		public void Execute(IList<Parameter> parameters)
		{
			executor(parameters);
		}
		
	}
}
