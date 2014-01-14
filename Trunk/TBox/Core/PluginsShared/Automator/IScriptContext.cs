using System;
using Mnk.Library.Common.MT;

namespace Mnk.TBox.Core.PluginsShared.Automator
{
	public interface IScriptContext
	{
        IUpdater Updater { get; }
        Action<Action> Sync { get; } 
	}
}
