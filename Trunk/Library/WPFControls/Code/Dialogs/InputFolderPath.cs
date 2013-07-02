using System;
using System.Windows;
using Common.Tools;
using WPFControls.Dialogs;
using MessageBox = System.Windows.MessageBox;

namespace WPFControls.Code.Dialogs
{
	public sealed class InputFolderPath : BaseDialog
	{
		private readonly string invalidPathTemplate;

		public InputFolderPath(string caption, PathTemplates templates,
			Func<string, bool> validator, Window owner=null) :
			base(caption, templates, validator, owner)
		{
			invalidPathTemplate = templates.InvalidPath;
		}

		public override bool Add(out string[] newNames)
		{
			var result = DialogsCache.ShowInputFolderPath(Caption, string.Empty, Owner);
			if (result.Key)
			{
				if (Validator(result.Value))
				{
					newNames = new[] { result.Value };
					return true;
				}
				MessageBox.Show(string.Format(invalidPathTemplate, result.Value), Caption,
								MessageBoxButton.OK, MessageBoxImage.Hand);
			}
			newNames = new string[0];
			return false;
		}

		public override bool Edit(string name, out string newName)
		{
			var result = DialogsCache.ShowInputFolderPath(Caption, name, Owner);
			if (result.Key)
			{
				if (Validator(result.Value) || name.EqualsIgnoreCase(result.Value))
				{
					newName = result.Value;
					return true;
				}
				MessageBox.Show(string.Format(invalidPathTemplate, result.Value), Caption,
								MessageBoxButton.OK, MessageBoxImage.Hand);
			}
			newName = string.Empty;
			return false;
		}

		public override bool Clone(string name, out string newName)
		{
			var result = DialogsCache.ShowInputFolderPath(Caption, name, Owner);
			if (result.Key)
			{
				if (Validator(result.Value))
				{
					newName = result.Value;
					return true;
				}
				MessageBox.Show(string.Format(invalidPathTemplate, result.Value), Caption,
								MessageBoxButton.OK, MessageBoxImage.Hand);
			}
			newName = string.Empty;
			return false;
		}
	}
}
