using System;
using Common.MT;

namespace PluginsShared.Automator
{
	public interface IScriptContext
	{
        IUpdater Updater { get; }
        Action<Action> Sync { get; } 
	}
}
