using System;
using Mnk.Library.Common.MT;

namespace Mnk.TBox.Core.PluginsShared.Automator
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
