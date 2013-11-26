using PluginsShared.ReportsGenerator;
using PluginsShared.ScriptEngine;
using ScriptEngine.Core;
using ScriptEngine.Core.Interfaces;

namespace SkyNet.Common.Scripts
{
    public class SkyNetScriptConfigurator : IScriptConfigurator
    {
        private readonly IScriptCompiler<IReportScript> compiler = new ScriptCompiler<IReportScript>();

        public ScriptPackage GetParameters(string scriptText)
        {
            return compiler.GetPackages(scriptText);
        }
    }
}
