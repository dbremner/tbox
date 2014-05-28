using System;
using System.Collections.Generic;
using System.Windows;
using Mnk.Library.WpfControls.Dialogs;

namespace Mnk.Library.WpfControls.Code.Dialogs
{
    public sealed class InputTextList : BaseDialog
    {
        private readonly bool isReadOnly;
        public IList<string> ItemsSource { get; set; }

        public InputTextList(string caption, Templates templates, Func<string, bool> validator, Func<Window> ownerGetter, bool isReadOnly = false) :
            base(caption, templates, validator, ownerGetter)
        {
            this.isReadOnly = isReadOnly;
            ItemsSource = new List<string>();
        }

        private bool BaseOp(string name, out string newName, string template)
        {
            var result = DialogsCache.ShowInputComboBox(template, Caption, name, Validator, ItemsSource, Owner, isReadOnly);
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
