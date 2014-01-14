using System;
using Mnk.Library.Common.Base;
using Mnk.Library.Common.Base.Log;
using Mnk.TBox.Locales.Localization.Plugins.HtmlPad;
using Mnk.Library.WPFControls.Tools;

namespace Mnk.TBox.Plugins.HtmlPad
{
	/// <summary>
	/// Interaction logic for Dialog.xaml
	/// </summary>
	public partial class Dialog 
	{
		private static readonly ILog Log = LogManager.GetLogger<Dialog>();
		public Dialog()
		{
			InitializeComponent();
			Title = HtmlPadLang.PluginName;
			Editor.Format = "html";
			Editor.TextChanged += EditorOnTextChanged;
		}

		private void EditorOnTextChanged(object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(Editor.Value))
			{
				ExceptionsHelper.HandleException(
					() => Html.NavigateToString(Editor.Value),
					() => "Can't set html body",
					Log
					);
			}
		}

		protected override void OnShow()
		{
			base.OnShow();
			Editor.SetFocus();
		}
	}
}
