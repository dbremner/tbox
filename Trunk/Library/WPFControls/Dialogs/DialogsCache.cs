﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Common.MT;
using WPFControls.Dialogs.Menu;

namespace WPFControls.Dialogs
{
	public static class DialogsCache
	{
		private static readonly IDictionary<Type, IList<IDialog>> Dialogs = new Dictionary<Type, IList<IDialog>>();

		public static void ShowProgress(Action<IUpdater> function, string title = "", Window owner = null, bool topmost = true, bool showInTaskBar= false, ImageSource icon = null)
		{
			GetDialog<ProgressDialog>().ShowDialog(function, title, owner, topmost, showInTaskBar, icon);
		}

		public static KeyValuePair<bool, string> ShowInputBox(string question, string caption, string value, Func<string, bool> validator, Window owner = null)
		{
			return GetDialog<InputTextBox>().ShowDialog(question, caption, value, validator, owner);
		}

		public static KeyValuePair<bool, string> ShowMemoBox(string question, string caption, string value, Func<string, bool> validator, Window owner = null)
		{
			return GetDialog<InputMemoBox>().ShowDialog(question, caption, value, validator, owner);
		}

		public static KeyValuePair<bool, string> ShowInputComboBox(string question, string caption, string value, Func<string, bool> validator, IList<string> source, Window owner = null)
		{
			return GetDialog<InputComboBox>().ShowDialog(question, caption, value, validator, source, owner);
		}

		public static KeyValuePair<bool, string> ShowInputSelect(string question, string caption, string value, Func<string, bool> validator, IList<string> source, Window owner = null, bool showInTaskBar = false)
		{
			return GetDialog<InputSelect>().ShowDialog(question, caption, value, validator, source, owner, showInTaskBar);
		}

		public static KeyValuePair<bool, string[]> ShowInputFilePath(string caption, string value, string filter="", bool allowSelectMany=false, Window owner = null)
		{
			return GetDialog<InputFilePath>().ShowDialog(caption, value, filter, allowSelectMany, owner);
		}

		public static KeyValuePair<bool, string> ShowInputFolderPath(string caption, string value, Window owner = null)
		{
			return GetDialog<InputFolderPath>().ShowDialog(caption, value, owner);
		}

		public static KeyValuePair<bool, string> ShowInputMenuItem(string question, string caption, string value, Func<string, bool> validator, IEnumerable<MenuDialogItem> values, Window owner = null, bool showInTaskBar = false)
		{
			return GetDialog<InputMenuItem>().ShowDialog(question, caption, value, validator, values, owner, showInTaskBar);
		}

		public static T GetDialog<T>()
			where T : IDialog
		{
			lock (Dialogs)
			{
				var type = typeof (T);
				IList<IDialog> list;
				if (!Dialogs.TryGetValue(type, out list))
				{
					Dialogs[type] = list = new List<IDialog>();
				}
				var available = list.FirstOrDefault(x => !x.IsVisible);
				if (available == null)
				{
					available = (IDialog)Activator.CreateInstance(type);
					list.Add(available);
				}
				return (T)available;
			}
		}

		public static void Dispose()
		{
			lock (Dialogs)
			{
				foreach (var d in Dialogs.SelectMany(dialog => dialog.Value))
				{
					d.Dispose();
				}
				Dialogs.Clear();
			}
		}
	}
}
