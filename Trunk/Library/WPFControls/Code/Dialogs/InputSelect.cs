using System;
using System.Collections.Generic;
using System.Windows;
using Mnk.Library.WpfControls.Dialogs;

namespace Mnk.Library.WpfControls.Code.Dialogs
{
	public sealed class InputSelect : BaseDialog
	{
		public IList<string> ItemsSource { get; set; }
        public InputSelect(string caption, Templates templates, Func<string, bool> validator, Func<Window> ownerGetter) :
			base(caption, templates, validator, ownerGetter)
		{
		}

		private bool BaseOp(string name, out string newName, string template)
		{
			var result = DialogsCache.ShowInputSelect(template, Caption, name, Validator, ItemsSource, Owner);
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
