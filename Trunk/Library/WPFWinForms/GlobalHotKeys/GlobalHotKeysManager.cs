using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using Mnk.Library.Common.Log;

namespace Mnk.Library.WpfWinForms.GlobalHotkeys
{
	public sealed class GlobalHotkeysManager : IDisposable
	{
		private readonly ILog log = LogManager.GetLogger<GlobalHotkeysManager>();

		private readonly Window window = new Window();
		private int currentId = 0;
		private readonly IDictionary<ModifierKeys, IDictionary<Key, Action>> 
			dictionary = new Dictionary<ModifierKeys, IDictionary<Key, Action>>();

		public GlobalHotkeysManager()
		{
			window.KeyPressed += WindowOnKeyPressed;
		}

		private void WindowOnKeyPressed(object sender, KeyPressedEventArgs args)
		{
			IDictionary<Key, Action> tmp;
			if (!dictionary.TryGetValue(args.GlobalHotkey.Modifier, out tmp)) return;
			Action action;
			if(tmp.TryGetValue(args.GlobalHotkey.Key, out action))
			{
				action();
			}
		}

		public void RegisterHotkey(GlobalHotkey globalHotkey, Action action)
		{
			var key = (Keys)KeyInterop.VirtualKeyFromKey(globalHotkey.Key);
			IDictionary<Key, Action> items;
			if (!dictionary.ContainsKey(globalHotkey.Modifier))
			{
				items = new Dictionary<Key, Action>();
				dictionary.Add(globalHotkey.Modifier, items);
			}
			else
			{
				items = dictionary[globalHotkey.Modifier];
			}
			if (!items.ContainsKey(globalHotkey.Key))
			{
				if (!NativeMethods.RegisterHotKey(window.Handle, currentId, (uint)globalHotkey.Modifier, (uint)key))
				{
					log.Write("Can't register hotkey: " + globalHotkey);
					return;
				}
				++currentId;
			}
			items[globalHotkey.Key] = action;
		}

		public void Dispose()
		{
			while (currentId >= 0) NativeMethods.UnregisterHotKey(window.Handle, currentId--);
			currentId = 0;
			dictionary.Clear();
			window.Dispose();
		}
	}
}
