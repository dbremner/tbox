using Mnk.TBox.Core.PluginsShared.ScriptEngine;

namespace Mnk.TBox.Core.PluginsShared.Automator
{
	public interface IScript
	{
		void Run(IScriptContext context);
	}
}
