using System;
using System.Windows.Media;
using Localization.Plugins.SqlWatcher;
using PluginsShared.Watcher;
using WPFControls.Dialogs;
using WPFSyntaxHighlighter;

namespace SqlWatcher.Code
{
	internal class LogDialog : ILogDialog
	{
		public MemoBox Dialog { get; private set; }
		public DialogWindow DialogWindow { get { return Dialog; } }

		public int EntriesCount
		{
			get { return Dialog.EntriesCount; }
		}

		public LogDialog(ImageSource icon)
		{
			Dialog = new MemoBox
			{
				Title = SqlWatcherLang.PluginName, 
				IsReadOnly = true, 
				Icon = icon
			};
		}

		public event EventHandler OnClear
		{
			add { Dialog.OnClear += value; }
			remove { Dialog.OnClear -= value; }
		}

		public void ShowLogs()
		{
			Dialog.ShowAndActivate();
		}

		public void Clear()
		{
			Dialog.Clear();
		}

		public void Write(string caption, string value)
		{
			Dialog.Write(caption, value);
		}
	}
}
