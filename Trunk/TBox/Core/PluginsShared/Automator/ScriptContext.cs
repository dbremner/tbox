using System;
using Common.MT;

namespace PluginsShared.Automator
{
    public class ScriptContext : IScriptContext
	{
        public IUpdater Updater { get; set; }
        public Action<Action> Sync { get; set; }

        public ScriptContext()
        {
            Updater = new ConsoleUpdater();
            Sync = a => a();
        }
	}
}
