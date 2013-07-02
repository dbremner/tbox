namespace WPFControls.Code.Dialogs
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
			add: Properties.Resources.AddPath,
			clone: Properties.Resources.ClonePath,
			edit: Properties.Resources.EditPath,
			del: Properties.Resources.DelPath,
			clear: Properties.Resources.ClearAllPathes,
			invalidPath: Properties.Resources.InvalidPath
			);
		}

	}
}
