using System;
using System.Windows;
using Mnk.Library.WpfControls.Dialogs;

namespace Mnk.Library.WpfControls.Code.Dialogs
{
	public sealed class InputDate : BaseDialog
	{
		public InputDate(string caption, Templates templates, Func<string, bool> validator, Func<Window> ownerGetter) :
            base(caption, templates, validator, ownerGetter)
		{
		}

		private bool BaseOp(string name, out string newName, string template)
		{
			var result = DialogsCache.ShowInputDate(template, Caption, name, Validator, Owner);
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
