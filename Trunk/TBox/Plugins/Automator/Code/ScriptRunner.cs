using System;
using System.Collections.Generic;
using System.IO;
using Common.MT;
using PluginsShared.Automator;
using PluginsShared.ScriptEngine;
using ScriptEngine;
using ScriptEngine.Core;
using ScriptEngine.Core.Interfaces;
using ScriptEngine.Core.Params;

namespace Automator.Code
{
	sealed class ScriptRunner : IScriptRunner
	{
	    private readonly IScriptCompiler<IScript> compiler = new ScriptCompiler<IScript>();

        public void Run(string path, IList<Parameter> parameters, Action<Action> dispatcher, IUpdater u)
		{
		    var s = compiler.Compile(File.ReadAllText(path), parameters);
            s.Run(new ScriptContext { Updater = u, Sync = dispatcher });
		}

        public ScriptPackage GetParameters(string scriptText)
		{
            return compiler.GetPackages(scriptText);
		}
	}
}
