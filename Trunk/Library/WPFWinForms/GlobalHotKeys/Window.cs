using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace Mnk.Library.WpfWinForms.GlobalHotkeys
{
	internal sealed class Window : NativeWindow, IDisposable
	{
		private const int WmHotkey = 0x0312;
		public event EventHandler<KeyPressedEventArgs> KeyPressed;

		public Window()
		{
			CreateHandle(new CreateParams());
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			if (m.Msg != WmHotkey) return;
			var wfkey = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
			var key = KeyInterop.KeyFromVirtualKey((int)wfkey);

			var modifier = (ModifierKeys)((int)m.LParam & 0xFFFF);

			if (KeyPressed != null)
				KeyPressed(this, new KeyPressedEventArgs(new GlobalHotkey(modifier, key)));
		}

		public void Dispose()
		{
			DestroyHandle();
		}
	}
}
