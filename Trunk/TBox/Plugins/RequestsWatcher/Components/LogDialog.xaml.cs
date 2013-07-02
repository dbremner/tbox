using System;
using System.Drawing;
using System.Windows;
using System.Windows.Threading;
using Common.Network;
using PluginsShared.Watcher;
using WPFControls.Dialogs;
using WPFControls.Drawings.DirectionsTable;
using WPFControls.Tools;
using Request = RequestsWatcher.Code.Request;

namespace RequestsWatcher.Components
{
	/// <summary>
	/// Interaction logic for LogDialog.xaml
	/// </summary>
	public partial class LogDialog : ILogDialog
	{
		private readonly DirectionsTable table = new DirectionsTable();
		private readonly DispatcherTimer timer = new DispatcherTimer();
		private bool needRedraw = false;
		public DialogWindow DialogWindow { get { return this; } }
		public int EntriesCount
		{
			get { return table.EntriesCount; }
		}

		public int MaxEntriesInLog
		{
			get { return table.MaxCount; }
			set { table.MaxCount = value; }
		}

		public LogDialog(Icon icon)
		{
			InitializeComponent();
			scrollView.Content = table;
			table.Selected += TableOnSelected;
			timer.Interval =new TimeSpan(0,0,1);
			timer.Tick += TimerOnTick;
			
			this.SetIcon(icon);
		}

		private void TimerOnTick(object sender, EventArgs eventArgs)
		{
			if (!needRedraw) return;
			table.Redraw();
			needRedraw = false;
		}

		private void TableOnSelected(object o, IDirectionable directionable)
		{
			var item = directionable as Request;
			Send.Value = item == null ? null : CreateInfo(item.Send);
			Receive.Value = item == null ? null : CreateInfo(item.Receive);
		}

		private static ResponseInfo CreateInfo(string text)
		{
			var id = text.IndexOf(Environment.NewLine + Environment.NewLine, StringComparison.Ordinal);
			if (id == -1)
			{
				return new ResponseInfo{Headers = text};
			}
			return new ResponseInfo
				{
					Headers = text.Substring(0, id),
					Body = text.Substring(id+2*Environment.NewLine.Length)
				};
		}

		public event Action OnClear
		{
			add { table.Cleared += value; }
			remove { table.Cleared -= value; }
		}

		public void ShowLogs()
		{
			needRedraw = false;
			table.Redraw();
			timer.Start();
			ShowAndActivate();
		}

		public void Clear()
		{
			table.Clear();
			table.Redraw();
		}

		public void Write(string caption, string value)
		{
			table.Add(ServiceStack.Text.JsonSerializer.DeserializeFromString<Request>(value) );
			needRedraw = true;
		}

		private void ButtonClearClick(object sender, RoutedEventArgs e)
		{
			Clear();
		}

		private void ButtonCloseClick(object sender, RoutedEventArgs e)
		{
			Close();
		}

		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			base.OnClosing(e);
			timer.Stop();
		}
	}
}
