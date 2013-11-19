using PluginsShared.ReportsGenerator;
using PluginsShared.ScriptEngine;
using ScriptEngine.Core;
using ScriptEngine.Core.Interfaces;

namespace TeamManager.Code.Scripts
{
	sealed class ValidatorScriptConfigurator : IScriptConfigurator
	{
        private readonly IScriptCompiler<IDayStatusStrategy> compiler = new ScriptCompiler<IDayStatusStrategy>();

	    public ScriptPackage GetParameters(string scriptText)
	    {
            return compiler.GetPackages(scriptText);
	    }
	}
}
