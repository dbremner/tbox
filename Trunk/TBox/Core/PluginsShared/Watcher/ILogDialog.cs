using System;
using WPFControls.Code.Log;
using WPFControls.Dialogs;

namespace PluginsShared.Watcher
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
