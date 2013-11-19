using ScriptEngine.Core;

namespace PluginsShared.ScriptEngine
{
    public interface IScriptConfigurator
	{
        ScriptPackage GetParameters(string scriptText);
	}
}
