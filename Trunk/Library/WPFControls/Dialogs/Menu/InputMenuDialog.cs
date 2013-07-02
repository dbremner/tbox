using System;
using System.Collections.Generic;
using System.Windows;
using WPFControls.Code.Dialogs;

namespace WPFControls.Dialogs.Menu
{
	public class InputMenuDialog: BaseDialog
	{
		public IList<MenuDialogItem> ItemsSource { get; set; }
		public InputMenuDialog(string caption, Templates templates, Func<string, bool> validator, Window owner = null) :
			base(caption, templates, validator, owner)
		{
		}

		private bool BaseOp(string name, out string newName, string template)
		{
			var result = DialogsCache.GetDialog<InputMenuItem>().ShowDialog(template, Caption, name, Validator, ItemsSource, Owner);
			if (result.Key)
			{
				newName = result.Value;
				return true;
			}
			newName = string.Empty;
			return false;
		}

		public override bool Add(out string[] newNames)
		{
			string tmp;
			var ret = BaseOp(string.Empty, out tmp, Buttons.Add);
			newNames = string.IsNullOrWhiteSpace(tmp) ? new string[0] : new[] { tmp };
			return ret;
		}

		public override bool Edit(string name, out string newName)
		{
			return BaseOp(name, out newName, Buttons.Edit);
		}
	}
}
