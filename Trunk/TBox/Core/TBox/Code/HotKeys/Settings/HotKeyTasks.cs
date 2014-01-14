using System;
using Mnk.Library.Common.Tools;
using Mnk.Library.Common.UI.ModelsContainers;

namespace Mnk.TBox.Core.Application.Code.HotKeys.Settings
{
	[Serializable]
	public class HotKeyTasks
	{
		public bool IsEnabled { get; set; }
		public CheckableDataCollection<HotKeyTask> Tasks { get; set; }

		public HotKeyTasks()
		{
			IsEnabled = false;
			Tasks = new CheckableDataCollection<HotKeyTask>();
		}

		public HotKeyTasks Clone()
		{
			return new HotKeyTasks
				{
					IsEnabled = IsEnabled,
					Tasks = Tasks.Clone()
				};
		}
	}
}
