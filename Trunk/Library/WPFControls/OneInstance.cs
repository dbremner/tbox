using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;

namespace Mnk.Library.WpfControls
{
	public static class OneInstance
	{
		private static class NativeMethods
		{
			[DllImport( "user32", CharSet = CharSet.Unicode)]
			private static extern int RegisterWindowMessage( string message );

			public static int RegisterWindowMessage( string format, params object[] args )
			{
				var message = String.Format(CultureInfo.InvariantCulture, format, args );
				return RegisterWindowMessage( message );
			}

			public const int HwndBroadcast = 0xffff;

			[DllImport("user32", CharSet = CharSet.Unicode)]
			public static extern bool PostMessage( IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam );
		}

		private static class ProgramInfo
		{
			static public string AssemblyGuid
			{
				get
				{
					var attributes = Assembly.GetEntryAssembly().GetCustomAttributes( typeof( GuidAttribute ), false );
					return attributes.Length == 0 ?
						String.Empty :
						( (GuidAttribute)attributes[0] ).Value;
				}
			}
		}

		private static readonly int WmShowfirstinstance =
			NativeMethods.RegisterWindowMessage( "WM_SHOWFIRSTINSTANCE|{0}", ProgramInfo.AssemblyGuid );

		public static class MainWindow
		{
			private static Window owner;
			public static bool Init(Window ownerWindow)
			{
				owner = ownerWindow;
				owner.SourceInitialized += WindowInitialized;
				return App.CreatedNew;
			}

			private static void WindowInitialized( object sender, EventArgs eventArgs )
			{
				var source = HwndSource.FromHwnd( new WindowInteropHelper( owner ).Handle );
				source.AddHook( WindowProc );
			}

			private static IntPtr WindowProc( IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam, ref bool Handled )
			{
				if ( Msg == WmShowfirstinstance )
				{
					owner.Show();
					owner.WindowState = WindowState.Normal;
					owner.Activate();
					owner.Focus();
				}
				return IntPtr.Zero;
			}
		}

		public static class App
		{
			private static volatile Mutex mutex;
			private static bool createdNew;
			public static bool CreatedNew { get { return createdNew; } }
			public static event EventHandler AfterExit;

			[STAThread]
			private static void Exit( object sender, ExitEventArgs e )
			{
				if (createdNew)
				{
					mutex.Dispose();
				}
				if (AfterExit != null) AfterExit(Application.Current, null);
			}

			private static void Startup( object sender, StartupEventArgs e )
			{
				if (createdNew) return;
				NativeMethods.PostMessage(
					(IntPtr)NativeMethods.HwndBroadcast,
					WmShowfirstinstance,
					IntPtr.Zero,
					IntPtr.Zero );
				Application.Current.Shutdown( 0 );
			}

			[STAThread]
			public static void Init(Application app)
			{
				var mutexName = String.Format(CultureInfo.InvariantCulture, "Global\\{0}", ProgramInfo.AssemblyGuid);
				mutex = new Mutex(true, mutexName, out createdNew);
				if (!createdNew)
				{
					mutex.Dispose();
				}
				app.Startup += Startup;
				app.Exit += Exit;
			}
		}

	}
}
