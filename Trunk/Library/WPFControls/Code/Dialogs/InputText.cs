using System;
using System.Windows;
using WPFControls.Dialogs;

namespace WPFControls.Code.Dialogs
{
	public sealed class InputText : BaseDialog
	{
		public InputText(string caption, Templates templates, Func<string, bool> validator, Window owner=null) :
			base(caption, templates, validator, owner)
		{
		}

		private bool BaseOp(string name, out string newName, string template)
		{
			var result = DialogsCache.ShowInputBox(template, Caption, name, Validator, Owner);
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
			newNames = string.IsNullOrWhiteSpace(tmp) ? new string[0] : new[] {tmp};
			return ret;
		}

		public override bool Edit(string name, out string newName)
		{
			return BaseOp(name, out newName, Buttons.Edit);
		}

	}
}
