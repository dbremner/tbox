using System;
using Mnk.Library.WPFControls.Code.Log;
using Mnk.Library.WPFControls.Dialogs;

namespace Mnk.TBox.Core.PluginsShared.Watcher
{
	public interface ILogDialog : ICaptionedLog
	{
		int EntriesCount { get; }
		event EventHandler OnClear;
		void ShowLogs();
		void Clear();
		DialogWindow DialogWindow { get; }
	}
}
