using System;
using System.Collections.Generic;
using Common.MT;
using PluginsShared.ReportsGenerator;
using PluginsShared.ScriptEngine;
using ScriptEngine.Core;
using ScriptEngine.Core.Interfaces;
using ScriptEngine.Core.Params;

namespace SkyNet.Common.Scripts
{
    public class SkyNetScriptRunner : IScriptRunner
    {
        private readonly IScriptCompiler<IReportScript> compiler = new ScriptCompiler<IReportScript>();

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
