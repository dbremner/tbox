using System;
using System.Drawing;
using PluginsShared.Watcher;
using WPFControls.Dialogs;
using WPFControls.Tools;
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

		public LogDialog(Icon icon)
		{
			Dialog = new MemoBox{Title = "SqlWatcher"};
			Dialog.SetIcon(icon);
		}

		public event Action OnClear
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
