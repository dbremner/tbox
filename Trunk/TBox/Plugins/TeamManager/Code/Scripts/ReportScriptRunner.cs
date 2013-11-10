using System;
using System.Collections.Generic;
using System.IO;
using Common.MT;
using PluginsShared.ReportsGenerator;
using PluginsShared.ScriptEngine;
using ScriptEngine.Core;
using ScriptEngine.Core.Interfaces;
using ScriptEngine.Core.Params;

namespace TeamManager.Code.Scripts
{
	sealed class ReportScriptRunner : IScriptRunner
	{
        private readonly IScriptCompiler<IReportScript> compiler = new ScriptCompiler<IReportScript>();

        public void Run(string path, IReportScriptContext context, ICollection<Parameter> parameters)
        {
            var s = compiler.Compile(File.ReadAllText(path), parameters);
            s.Run(context);
        }

        public void Run(string path, IList<Parameter> parameters, Action<Action> dispatcher, IUpdater u)
	    {
            throw new NotImplementedException();
	    }

	    public ScriptPackage GetParameters(string scriptText)
	    {
            return compiler.GetPackages(scriptText);
	    }
	}
}
