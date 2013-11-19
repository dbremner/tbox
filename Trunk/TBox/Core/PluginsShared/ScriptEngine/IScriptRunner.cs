using System;
using System.Collections.Generic;
using Common.MT;
using ScriptEngine.Core.Params;

namespace PluginsShared.ScriptEngine
{
    public interface IScriptRunner : IScriptConfigurator
	{
        void Run(string path, IList<Parameter> parameters, Action<Action> dispatcher, IUpdater u);
	}
}
