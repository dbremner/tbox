﻿using Common.Base;
using Common.Base.Log;
using WPFControls.Tools;

namespace HtmlPad
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
			Editor.Format = "html";
			Editor.TextChanged += EditorOnTextChanged;
		}

		private void EditorOnTextChanged()
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
