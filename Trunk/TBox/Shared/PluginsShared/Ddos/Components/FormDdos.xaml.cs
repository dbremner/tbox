using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using PluginsShared.Ddos.Settings;
using PluginsShared.Ddos.Statistic;
using WPFControls.Tools;
using WPFWinForms;

namespace PluginsShared.Ddos.Components
{
	/// <summary>
	/// Interaction logic for FormDdos.xaml
	/// </summary>
	public partial class FormDdos 
	{
		private IDdoser ddoser;
		private Analizer analizer;
		private readonly TrayIcon trayIcon = new TrayIcon();
		private readonly DispatcherTimer viewTimer;

		public FormDdos()
		{
			InitializeComponent();
			SetGraphic(null);
			Panel.View = Operations;
			Panel.CheckChangedByPanel += CheckBoxChecked;
			trayIcon.MouseClick += TrayIconOnMouseClick;
			viewTimer = new DispatcherTimer();
			viewTimer.Tick += OnViewTimer;
			viewTimer.Interval = new TimeSpan(0,0,0,1);
		}

		public override void Dispose()
		{
			ddoser.Stop();
			trayIcon.Dispose();
			base.Dispose();
		}

		private void SetGraphic(GraphicsInfo graphicsInfo)
		{
			Graph.Clear();
			if (graphicsInfo == null) return;
			Graph.AddGrapic(graphicsInfo.MaxTime);
			Graph.AddGrapic(graphicsInfo.AverageTime);
			Graph.AddGrapic(graphicsInfo.MinTime);
		}

		private void OnViewTimer(object sender, EventArgs eventArgs)
		{
			if (!viewTimer.IsEnabled || !ddoser.IsWorks) return;
			switch (Tabs.SelectedIndex)
			{
				case 1:
					RedrawGraphic();
					break;
				case 2:
					OnSelectedStatisticChanged(null, null);
					break;
			}
		}

		private void RedrawGraphic()
		{
			lock (Graph)
			{
				Graph.Redraw();
			}
		}

		private void TrayIconOnMouseClick(MouseButton obj)
		{
			ShowAndActivate();
		}

		public void Init(Icon icon, IDdoser ddoser)
		{
			this.SetIcon(icon);
			trayIcon.Icon = icon;
			this.ddoser = ddoser;
		}

		private void FillComboBoxes(ComboBox cb)
		{
			cb.Items.Clear();
			foreach (var key in analizer.Keys)
			{
				cb.Items.Add(key);
			}
			cb.SelectedIndex = 0;
		}

		private static void AppendResult(StringBuilder sb, string property, double value)
		{
			sb.AppendFormat("{0}: \t {1:0.##}", property, value).AppendLine();
		}

		private static void AppendResult(StringBuilder sb, object property, object value)
		{
			sb.AppendFormat("{0}: \t {1}", property, value).AppendLine();
		}

		public void ShowDialog(IProfile p)
		{
			if(IsVisible || ddoser.IsWorks)return;
			Title = "Ddos - [" + p.Key + "]";
			analizer = null;
			Statistic.Items.Clear();
			Graphics.Items.Clear();
			SetGraphic(null);
			Results.Text = string.Empty;
			DataContext = p;
			CheckBoxChecked(Operations, null);
			ShowAndActivate();
		}

		private void CheckBoxChecked(object sender, RoutedEventArgs e)
		{
			Operations.OnCheckChangedEvent(sender, e);
			btnStartStop.IsEnabled = ((IProfile) DataContext).GetOperations().Any();
			btnCopy.IsEnabled = (analizer!=null) && btnStartStop.IsEnabled;
		}

		private void BtnStartClick(object sender, RoutedEventArgs e)
		{
			btnStartStop.IsEnabled = false;
			if (ddoser.IsWorks)
			{
				Stop();
			}
			else
			{
				Start();
			}
			btnStartStop.IsEnabled = true;
		}

		private void Start()
		{
			EnableControls(false);
			Results.Text = string.Empty;
			var p = ((IProfile) DataContext);
			btnStartStop.Content = "Stop";
			trayIcon.HoverText = Title;
			trayIcon.IsVisible = true;
			var ops = p.GetOperations().ToArray();
			analizer = new Analizer(ops.Select(x => x.Key));
			SetGraphic(analizer.GetGraphic(string.Empty));
			FillComboBoxes(Statistic);
			FillComboBoxes(Graphics);
			ddoser.Start(ops, analizer);
			Graph.StartTime = DateTime.Now;
			Graph.EndTime = DateTime.MinValue;
		}

		private void EnableControls(bool state)
		{
			Operations.IsEnabled = Panel.IsEnabled = btnCopy.IsEnabled = state;
		}

		public void Stop()
		{
			Graph.EndTime = DateTime.Now;
			ddoser.Stop();
			btnStartStop.Content = "Start";
			trayIcon.IsVisible = false;
			EnableControls(true);
		}

		private void OnSelectedStatisticChanged(object sender, SelectionChangedEventArgs e)
		{
			ShowStatistic((Statistic.SelectedValue??string.Empty).ToString());
		}

		private void ShowStatistic(string key)
		{
			if (analizer == null) return;
			var s = analizer.GetStatistic(key);
			var sb = new StringBuilder();
			AppendResult(sb, "Count", s.Count);
			AppendResult(sb, "Time", s.Time/1000.0);
			AppendResult(sb, "AverageTime", (s.Count == 0) ? 0 : (s.Time/(float) s.Count/1000.0));
			AppendResult(sb, "MinTime", s.MinTime/1000.0);
			AppendResult(sb, "MaxTime", s.MaxTime/1000.0);
			sb.AppendLine();
			foreach (var status in s.Statutes)
			{
				AppendResult(sb, status.Key,
				             string.Format("{0}  ( {1:0.##} )%", status.Value, 100*status.Value/(float) s.Count));
			}
			Results.Text = sb.ToString();
		}

		private void OnSelectedGraphicChanged(object sender, SelectionChangedEventArgs e)
		{
			DrawGraphic((Graphics.SelectedValue??string.Empty).ToString());
		}

		private void DrawGraphic(string key)
		{
			if (analizer == null) return;
			SetGraphic(analizer.GetGraphic(key));
			RedrawGraphic();
		}

		private void BtnCopyClick(object sender, RoutedEventArgs e)
		{
			if (analizer == null) return;
			var sb = new StringBuilder();
			foreach (var key in new[]{""}.Concat(analizer.Keys))
			{
				AppendValues(sb, key, "Count", x => x.Count);
				AppendValues(sb, key, "Min", x => x.MinTime);
				AppendValues(sb, key, "Avg", x => x.Time / (float)x.Count);
				AppendValues(sb, key, "Max", x => x.MaxTime);
				sb.AppendLine();
			}
			Clipboard.SetText(sb.ToString());
		}

		private void AppendValues(StringBuilder sb, string key, string prefix, Func<OperationStatistic, double> getter)
		{
			const char divider = '\t';
			sb.Append(key).Append(divider).Append(prefix).Append(divider);
			foreach (var s in analizer.GetValues(key))
			{
				if (s.Count <= 0)
				{
					sb.Append(string.Empty);
				}
				else
				{
					sb.Append(getter(s));
				}
				sb.Append(divider);
			}
			sb.AppendLine();
		}

		protected override void OnShow()
		{
			base.OnShow();
			viewTimer.Start();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			viewTimer.Stop();
			base.OnClosing(e);
		}

		private void CancelClick(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
