using System;
using System.Collections.Generic;
using System.Windows;
using Mnk.Library.Common.Tools;
using Mnk.Library.Localization.WPFControls;
using Mnk.Library.WpfControls.Tools;

namespace Mnk.Library.WpfControls.Dialogs
{
    /// <summary>
    /// Interaction logic for InputTextBox.xaml
    /// </summary>
    sealed partial class InputDateBox
    {
        private bool isSuccess;
        private string initValue;
        private Func<string, bool> validator;

        public InputDateBox()
        {
            InitializeComponent();
            edData.ValueChanged += TextChanged;
        }

        public string Value
        {
            get { return edData.Value.HasValue ? edData.Value.Value.ToShortDateString() : string.Empty; }
            set
            {
                DateTime date;
                if (DateTime.TryParse(value, out date))
                {
                    edData.Value = date;
                }
                else
                {
                    edData.Value = null;
                }
            }
        }

        public KeyValuePair<bool, string> ShowDialog(string question, string caption, string value, Func<string, bool> validator, Window owner)
        {
            btnOk.ToolTip = string.Empty;
            initValue = value??string.Empty;
            this.validator = validator ?? (v => !v.EqualsIgnoreCase(initValue.Trim()));
            Value = value;
            Owner = owner;
            Icon = owner == null ? null : owner.Icon;
            Title = caption;
            edData.Caption = question;
            TextChanged(null, null);
            isSuccess = false;
            TextChanged(null, null);
            edData.SetFocus();
            ShowDialog();
            return new KeyValuePair<bool, string>(isSuccess, Value);
        }

        private void BtnOk_OnClick(object sender, EventArgs e)
        {
            isSuccess = true;
            DialogResult = true;
        }

        private void TextChanged(object sender, EventArgs e)
        {
            var value = Value.Trim();
            btnOk.IsEnabled = (value.Length > 0) && validator(value);
            btnOk.ToolTip = btnOk.IsEnabled ? "" : WPFControlsLang.ValueAlreadyExistOrIncorrect;
        }

    }
}
