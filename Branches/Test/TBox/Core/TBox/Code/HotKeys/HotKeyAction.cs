using System;
using TBox.Code.HotKeys.Settings;

namespace TBox.Code.HotKeys
{
	class HotKeyAction : HotKeyTask
	{
		public Action Action { get; set; }
	}
}
