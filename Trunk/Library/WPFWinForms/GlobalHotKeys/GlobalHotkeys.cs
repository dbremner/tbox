using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;
using Common.Base.Log;

namespace WPFWinForms.GlobalHotKeys
{
	public sealed class GlobalHotkeys : IDisposable
	{
		private static ILog Log = LogManager.GetLogger<GlobalHotkeys>();

		[DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

		[DllImport("user32.dll")]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		private readonly Window window = new Window();
		private int currentId = 0;
		private readonly IDictionary<ModifierKeys, IDictionary<Key, Action>> 
			dictionary = new Dictionary<ModifierKeys, IDictionary<Key, Action>>();

		public GlobalHotkeys()
		{
			window.KeyPressed += WindowOnKeyPressed;
		}

		private void WindowOnKeyPressed(object sender, KeyPressedEventArgs args)
		{
			IDictionary<Key, Action> tmp;
			if (!dictionary.TryGetValue(args.HotKey.Modifier, out tmp)) return;
			Action action;
			if(tmp.TryGetValue(args.HotKey.Key, out action))
			{
				action();
			}
		}

		public void RegisterHotKey(HotKey hotKey, Action action)
		{
			var key = (Keys)KeyInterop.VirtualKeyFromKey(hotKey.Key);
			IDictionary<Key, Action> dict;
			if (!dictionary.ContainsKey(hotKey.Modifier))
			{
				dict = new Dictionary<Key, Action>();
				dictionary.Add(hotKey.Modifier, dict);
			}
			else
			{
				dict = dictionary[hotKey.Modifier];
			}
			if (!dict.ContainsKey(hotKey.Key))
			{
				if (!RegisterHotKey(window.Handle, currentId, (uint) hotKey.Modifier, (uint) key))
				{
					Log.Write("Can't register hotkey: " + hotKey);
					return;
				}
				++currentId;
			}
			dict[hotKey.Key] = action;
		}

		public void Dispose()
		{
			while (currentId >= 0)UnregisterHotKey(window.Handle, currentId--);
			currentId = 0;
			dictionary.Clear();
			window.Dispose();
		}
	}
}
