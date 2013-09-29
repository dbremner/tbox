using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Common.Base;
using Common.Base.Log;
using WPFControls.Code.OS;
using WPFWinForms;

namespace TBox
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private static readonly ILog Log = LogManager.GetLogger<App>();
		private static bool handled = false;

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool SetDllDirectory(string path);

		public App()
		{
			var unmanagedLibsFolder = 
				Path.Combine(
					AppDomain.CurrentDomain.BaseDirectory,
					"Libraries",
					(IntPtr.Size == 8 ? "x64" : "x86")
				);
			SetDllDirectory(unmanagedLibsFolder);
			AppDomain.CurrentDomain.AssemblyResolve += (o, e) => LoadFromLibFolder(o, e, unmanagedLibsFolder);

			ShutdownMode = ShutdownMode.OnMainWindowClose;
			FormsStyles.Enable();
			AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
			DispatcherUnhandledException += CurrentDispatcherUnhandledException;
			Dispatcher.UnhandledException += DispatcherOnUnhandledException;
			TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

			OneInstance.App.Init(this);
		}

		static Assembly LoadFromLibFolder(object sender, ResolveEventArgs args, string unmanagedLibsFolder)
		{
			var assemblyPath = Path.GetFullPath(Path.Combine(unmanagedLibsFolder, new AssemblyName(args.Name).Name + ".dll"));
			return File.Exists(assemblyPath) ? Assembly.LoadFrom(assemblyPath) : null;
		}

		private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
		{
			LogException(e.Exception);
		}

		private void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			LogException(e.Exception);
		}

		private void CurrentDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			LogException(e.Exception);
		}

		void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			LogException(e.ExceptionObject);
		}

		void LogException(object ex)
		{
			if(handled)return;
			handled = true;
			const string message =
				"Sorry, unhandled exception occured. Application will be terminated.\nPlease contact with author to fix this issue.\nYou can try restart application to continue working...";
			if(ex is Exception)
			{
				Log.Write((Exception)ex, message);
			}
			else Log.Write(message);
			ExceptionsHelper.HandleException(DoExit, x=>{});
			Shutdown(-1);
		}

		private static void DoExit()
		{
			var w = Current.MainWindow as MainWindow;
			if (w != null)
			{
				w.MenuClose(true);
			}
		}
	}
}
