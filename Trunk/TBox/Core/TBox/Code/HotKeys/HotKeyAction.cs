using System;
using Mnk.TBox.Core.Application.Code.HotKeys.Settings;

namespace Mnk.TBox.Core.Application.Code.HotKeys
{
	class HotKeyAction : HotKeyTask
	{
		public Action Action { get; set; }
	}
}
