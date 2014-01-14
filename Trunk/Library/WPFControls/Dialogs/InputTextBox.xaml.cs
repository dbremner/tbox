using System;
using System.Collections.Generic;
using System.Windows;
using Mnk.Library.Common.Tools;
using Mnk.Library.Localization.WPFControls;
using Mnk.Library.WPFControls.Tools;

namespace Mnk.Library.WPFControls.Dialogs
{
	/// <summary>
	/// Interaction logic for InputTextBox.xaml
	/// </summary>
	sealed partial class InputTextBox 
	{
		private bool isSuccess;
		private string initValue;
		private Func<string, bool> validator;

		public InputTextBox()
		{
			InitializeComponent();
			edData.TextChanged += TextChanged;
		}

		public string Value
		{
			get { return edData.Text; }
			set { edData.Text = value.Trim(); }
		}

		public KeyValuePair<bool, string> ShowDialog( string question, string caption, string value, Func<string, bool> validator, Window owner )
		{
		    btnOk.ToolTip = string.Empty;
			initValue = value;
			Value = value;
			Owner = owner;
			Icon = owner == null ? null : owner.Icon;
			this.validator = validator;
			Title = caption;
			lbCaption.Content = question;
			TextChanged(null, null);
			isSuccess = false;
			edData.SetFocus();
			ShowDialog();
			Value = edData.Text.Trim();
			return new KeyValuePair<bool, string>(isSuccess, Value);
		}

		private void BtnOk_OnClick( object sender, EventArgs e )
		{
			isSuccess = true;
			DialogResult = true;
		}

		private void TextChanged( object sender, EventArgs e )
		{
			var value = Value;
			btnOk.IsEnabled = ( value.Length > 0 ) && 
				( validator != null && (validator( value ) || value.EqualsIgnoreCase(initValue) ));
            btnOk.ToolTip = btnOk.IsEnabled ? "" : WPFControlsLang.ValueAlreadyExistOrIncorrect;
		}

	}
}
