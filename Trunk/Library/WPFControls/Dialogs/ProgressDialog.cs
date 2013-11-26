using System;
using System.Windows;
using System.Windows.Media;
using Common.MT;
using WPFControls.Components.Updater;

namespace WPFControls.Dialogs
{
    class ProgressDialog : ClosableDialogWindow, IDialog
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
			ShowInTaskbar = false;
			ResizeMode = ResizeMode.NoResize;
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
				progress.ButtonClick(null, null);
			}
			base.OnClosing(e);
		}

		private Action<IUpdater> func;
		public bool? ShowDialog(Action<IUpdater> function, string title = "", Window owner = null, bool topmost = true, bool showInTaskBar = false, ImageSource icon = null)
		{
			Icon = icon ?? (owner == null ? null : owner.Icon);
			ShowInTaskbar = showInTaskBar;
			Topmost = topmost;
			Owner = owner;
			Title = title;
			func = function;
            progress.Start(func, Dispose);
			return ShowDialog();
		}

	    public void Dispose()
		{
			CanClose = true;
			Close();
		}
	}
}
