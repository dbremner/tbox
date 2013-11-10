using PluginsShared.ScriptEngine;

namespace PluginsShared.Automator
{
	public interface IScript
	{
		void Run(IScriptContext context);
	}
}
