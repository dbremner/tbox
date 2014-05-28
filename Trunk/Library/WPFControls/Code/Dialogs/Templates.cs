using Mnk.Library.Localization.WPFControls;

namespace Mnk.Library.WpfControls.Code.Dialogs
{
	public class Templates
	{
		public string Add { get; private set; }
		public string Edit { get; private set; }
		public string Clone { get; private set; }
		public string Del { get; private set; }
		public string Clear { get; private set; }

		public Templates(string add = "", string clone = "", string edit = "", string del = "", string clear = "")
		{
			Clear = clear;
			Del = del;
			Clone = clone;
			Edit = edit;
			Add = add;
		}

		public static Templates Default { get; private set; }
		static Templates()
		{
			Default = new Templates(
			add: WPFControlsLang.AddItem,
			clone: WPFControlsLang.CloneItem,
			edit: WPFControlsLang.EditItem,
			del: WPFControlsLang.DelItem,
			clear: WPFControlsLang.ClearAllItems
			);
		}
	}
}
