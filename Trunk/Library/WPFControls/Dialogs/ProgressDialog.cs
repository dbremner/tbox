using System;
using System.Windows;
using Common.MT;
using WPFControls.Code.OS;
using WPFControls.Components.Updater;

namespace WPFControls.Dialogs
{
	class ProgressDialog : Window, IDialog
	{
	    private bool shouldStart = false;
		private readonly Progress progress = new Progress{Margin = new Thickness(5)};
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
			IsVisibleChanged += OnVisibleChanged;
			Background = SystemColors.ControlBrush;
			Content = progress;
			Topmost = true;
		}
		
		public void SetMessage(string value)
		{
			progress.SetMessage(value);
		}
		public double Value { get { return progress.Value; } }
		public IProgressModel ProgressModel
		{
			get { return progress.ProgressModel; }
			set { progress.ProgressModel = value; }
		}

		protected bool CanClose { get; set; }

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
		    shouldStart = true;
			progress.Reset();
			return base.ShowDialog();
		}

		private void OnVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if(IsVisible && shouldStart)
			{
				progress.Start( func, OnProgressEnd );
			    shouldStart = false;
			}
		}

		public void OnProgressEnd()
		{
			Mt.Do(this, Hide);
		}

		public void Dispose()
		{
			CanClose = true;
			Close();
		}
	}
}
