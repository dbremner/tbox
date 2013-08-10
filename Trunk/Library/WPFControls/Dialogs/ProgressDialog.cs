using System;
using System.Windows;
using Common.MT;
using WPFControls.Code.OS;
using WPFControls.Components.Updater;

namespace WPFControls.Dialogs
{
	class ProgressDialog : Window, IDialog
	{
		private readonly Progress progress = new Progress{Margin = new Thickness(5)};
		public double Value { get { return progress.Value; } }
		protected bool CanClose { get; set; }

		public ProgressDialog()
		{
			CanClose = false;
			Width = 400;
			Height = 104;
			MaxHeight = 104;
			MinHeight = 104;
			MinWidth = 200;
			WindowStartupLocation = WindowStartupLocation.CenterOwner;
			ShowInTaskbar = false;
			WindowStyle = WindowStyle.ToolWindow;
			Background = SystemColors.ControlBrush;
			Content = progress;
			Topmost = true;
		}
		
		public void SetMessage(string value)
		{
			progress.SetMessage(value);
		}

		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			if (!CanClose)
			{
				e.Cancel = true;
				progress.Button_Click(null, null);
			}
			base.OnClosing(e);
		}

		private Action<IUpdater> func;
		public bool? ShowDialog(Action<IUpdater> function, string title = "", Window owner = null, bool topmost = true, bool showInTaskBar = false)
		{
			ShowInTaskbar = showInTaskBar;
			Topmost = topmost;
			Owner = owner;
			Title = title;
			func = function;
			progress.Reset();
			progress.Start(func, () => Mt.Do(this, Hide));
			return base.ShowDialog();
		}

		public void Dispose()
		{
			CanClose = true;
			Close();
		}
	}
}
