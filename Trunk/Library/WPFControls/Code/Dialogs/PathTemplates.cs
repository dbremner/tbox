using Mnk.Library.Localization.WPFControls;

namespace Mnk.Library.WpfControls.Code.Dialogs
{
	public class PathTemplates : Templates
	{
		public string InvalidPath { get; private set; }

		public PathTemplates(string add = "", string clone = "", string edit = "", string del = "", string clear = "", string invalidPath = "")
			: base(add, clone, edit, del, clear)
		{
			InvalidPath = invalidPath;
		}

		public new static PathTemplates Default { get; private set; }
		static PathTemplates()
		{
			Default = new PathTemplates(
			add: WPFControlsLang.AddPath,
			clone: WPFControlsLang.ClonePath,
			edit: WPFControlsLang.EditPath,
			del: WPFControlsLang.DelPath,
			clear: WPFControlsLang.ClearAllPaths,
			invalidPath: WPFControlsLang.InvalidPath
			);
		}

	}
}
