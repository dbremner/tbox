using System;
using System.Collections.Generic;
using Common.MT;
using ScriptEngine.Core;
using ScriptEngine.Core.Params;

namespace PluginsShared.ScriptEngine
{
	public interface IScriptRunner
	{
        void Run(string path, IList<Parameter> parameters, Action<Action> dispatcher, IUpdater u);
        ScriptPackage GetParameters(string scriptText);
	}
}
