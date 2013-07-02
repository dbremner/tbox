﻿using System.Linq;
using System.Text;
using System.Windows;
using Common.MT;
using SqlRunner.Code;
using SqlRunner.Code.Settings;
using WPFControls.Code;
using WPFControls.Code.OS;
using WPFControls.Dialogs;
using WPFControls.Dialogs.StateSaver;
using WPFSyntaxHighlighter;

namespace SqlRunner.Components
{
	/// <summary>
	/// Interaction logic for FormBatcg.xaml
	/// </summary>
	public sealed partial class FormBatch
	{
		private Profile profile;
		private Config config;
		private readonly LazyDialog<MemoBox> memobox = new LazyDialog<MemoBox>(()=>new MemoBox(), "message");
		public FormBatch()
		{
			InitializeComponent();
			PanelOps.View = Ops;
			PanelConnections.View = ConnectionStrings;
		}

		public void ShowDialog(Profile profile, Config config)
		{
			if(IsVisible)return;
			this.profile = profile;
			this.config = config;
			Title = "Batch run: [" + profile.Key + "]";
			Ops.ItemsSource = profile.Ops;
			ConnectionStrings.ItemsSource = config.ConnectionStrings;
			OnCheckChanged(null, null);
			ShowAndActivate();
		}

		private void StartClick(object sender, RoutedEventArgs e)
		{
			DialogsCache.ShowProgress(Work, "Run scripts", this);
		}

		private void Work(IUpdater u)
		{
			var ops = profile.Ops.CheckedItems.ToArray();
			var connections = config.ConnectionStrings.CheckedItems;
			var sb = new StringBuilder();
			foreach (var str in connections)
			{
				foreach (var op in ops)
				{
					if(u.UserPressClose)return;
					var r = BaseExecutor.GetResult(str.Key, op);
					sb.AppendFormat("[{0}] {1}", str.Key, op.Key)
						.AppendLine()
						.AppendFormat("Time:{0}, status: {1}", r.Time, r.Status)
						.AppendLine()
						.AppendFormat("Body: {0}", r.Body)
						.AppendLine()
						.AppendLine();
				}
			}
			Mt.Do(this, 
				()=>memobox.Do(a=>Mt.Do(this, a), d=>d.ShowDialog(Title, sb.ToString()), config.States ));
		}

		public void Save(Config config)
		{
			memobox.SaveState(config.States);
		}

		private void CancelClick(object sender, RoutedEventArgs e)
		{
			Hide();
		}

		private void OnCheckChanged(object sender, RoutedEventArgs e)
		{
			btnStart.IsEnabled = profile.Ops.CheckedValuesCount > 0 && config.ConnectionStrings.CheckedValuesCount>0;
		}

		public override void Dispose()
		{
			memobox.Dispose();
			base.Dispose();
		}

	}
}
