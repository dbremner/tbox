using System;
using System.Collections.Generic;
using TBox.Code.AutoUpdate;
using TBox.Code.ErrorsSender;
using TBox.Code.FastStart.Settings;
using TBox.Code.HotKeys.Settings;
using TBox.Code.Shelduler.Settings;
using WPFControls.Dialogs.StateSaver;

namespace TBox.Code
{
	[Serializable]
	public sealed class Config
	{
		public bool HideOnSave { get; set; }
		public bool HideOnCancel { get; set; }
		public bool StartHidden { get; set; }
		public bool UseMenuWithIcons { get; set; }
		public bool ShowSettingsByTraySingleClick { get; set; }
		public string Theme { get; set; }

		public Update Update { get; set; }
		public HotKeyTasks HotKeys { get; set; }
		public SchedulerTasks SchedulerTasks { get; set; }
		public List<string> DisabledItems { get; set; }
		public ErrorReports ErrorReports { get; set; }
		public DialogState DialogState { get; set; }
		public FastStartConfig FastStartConfig { get; set; }
		public bool UpdateFromSharedlFolder { get; set; }
		public string LastKnownVersion { get; set; }

		public Config()
		{
			HideOnSave = false;
			HideOnCancel = false;
			StartHidden = false;
			Update = new Update();
			HotKeys = new HotKeyTasks();
			SchedulerTasks  = new SchedulerTasks();
			DisabledItems = new List<string>();
			ErrorReports = new ErrorReports();
			FastStartConfig = new FastStartConfig();
			Theme = "Default.xaml";
			UseMenuWithIcons = true;
			ShowSettingsByTraySingleClick = true;
			UpdateFromSharedlFolder = false;
		}
	}
}
