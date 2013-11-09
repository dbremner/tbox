﻿using System;
using System.Linq;
using System.Windows;
using Common.Tools;
using WPFControls.Dialogs;
using MessageBox = System.Windows.MessageBox;

namespace WPFControls.Code.Dialogs
{
	public class InputFilePath : BaseDialog
	{
		private readonly string invalidPathTemplate;

		public InputFilePath(string caption, PathTemplates templates,
            Func<string, bool> validator, Func<Window> ownerGetter) :
			base(caption, templates, validator, ownerGetter)
		{
			this.invalidPathTemplate = templates.InvalidPath;
		}

		public override bool Add(out string[] newNames)
		{
			var isOk = false;
            var result = DialogsCache.ShowInputFilePath(Caption, string.Empty, Owner, string.Empty, true);
			if (result.Key)
			{
				isOk = true;
				foreach (var name in result.Value.Where(name => !Validator(name)))
				{
					MessageBox.Show(string.Format(invalidPathTemplate, name), Caption,
									MessageBoxButton.OK, MessageBoxImage.Hand);
					isOk = false;
					break;
				}
			}
			newNames = isOk ? result.Value : new string[0];
			return isOk;
		}

		public override bool Edit(string name, out string newName)
		{
            var result = DialogsCache.ShowInputFilePath(Caption, name, Owner, string.Empty, false);
			if (result.Key)
			{
				var selected = result.Value.FirstOrDefault();
				if (Validator(selected) || name.EqualsIgnoreCase(selected))
				{
					newName = selected;
					return true;
				}
				MessageBox.Show(string.Format(invalidPathTemplate, selected), Caption,
						MessageBoxButton.OK, MessageBoxImage.Hand);
			}
			newName = string.Empty;
			return false;
		}
	}
}