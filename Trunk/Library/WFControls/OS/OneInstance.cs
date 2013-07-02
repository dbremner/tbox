using System;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WFControls.OS
{
	public class OneInstance : IDisposable
	{
		public class SingleAppBase : Form
		{
			protected override void WndProc(ref Message m)
			{
				if (m.Msg == m_wmShowfirstinstance)
				{
					WinApi.ShowToFront(m.HWnd);
				}
				base.WndProc(ref m);
			}
		}

		private static class WinApi
		{
			[DllImport("user32")]
			private static extern int RegisterWindowMessage(string message);

			public static int RegisterWindowMessage(string format, params object[] args)
			{
				string message = String.Format(format, args);
				return RegisterWindowMessage(message);
			}

			public const int HwndBroadcast = 0xffff;
			private const int m_swShownormal = 1;

			[DllImport("user32")]
			public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

			[DllImportAttribute("user32.dll")]
			private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

			[DllImportAttribute("user32.dll")]
			private static extern bool SetForegroundWindow(IntPtr hWnd);

			public static void ShowToFront(IntPtr window)
			{
				ShowWindow(window, m_swShownormal);
				SetForegroundWindow(window);
			}
		}

		private static class ProgramInfo
		{
			static public string AssemblyGuid
			{
				get
				{
					object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(GuidAttribute), false);
					if (attributes.Length == 0)
					{
						return String.Empty;
					}
					return ((GuidAttribute)attributes[0]).Value;
				}
			}
		}

		private static void ShowFirstInstance()
		{
			WinApi.PostMessage(
				(IntPtr)WinApi.HwndBroadcast,
				m_wmShowfirstinstance,
				IntPtr.Zero,
				IntPtr.Zero);
		}

		private static readonly int m_wmShowfirstinstance =
			WinApi.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", ProgramInfo.AssemblyGuid);
		private readonly Mutex m_mutex;
		private readonly bool m_onlyInstance = false;
		public OneInstance()
		{
			string mutexName = String.Format("Global\\{0}", ProgramInfo.AssemblyGuid);
			m_mutex = new Mutex(true, mutexName, out m_onlyInstance);
			if(!m_onlyInstance)
			{
				ShowFirstInstance();
			}
		}
		public bool IsOne
		{
			get { return m_onlyInstance; }
		}

		public void Dispose()
		{
			if (m_onlyInstance)
			{
				m_mutex.ReleaseMutex();
			}
		}
	}
}
