using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Common.Base;
using Common.Base.Log;
using Localization.TBox;
using WPFControls.Code.OS;
using WPFControls.Localization;
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
			Translator.Culture = new CultureInfo("en");
			var unmanagedLibsFolder = 
				Path.Combine(
					AppDomain.CurrentDomain.BaseDirectory,
					"Libraries",
					(IntPtr.Size == 8 ? "x64" : "x86")
				);
			SetDllDirectory(unmanagedLibsFolder);
			AppDomain.CurrentDomain.AssemblyResolve += LoadFromLibFolder;

			ShutdownMode = ShutdownMode.OnMainWindowClose;
			FormsStyles.Enable();
			AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
			DispatcherUnhandledException += CurrentDispatcherUnhandledException;
			Dispatcher.UnhandledException += DispatcherOnUnhandledException;
			TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

			OneInstance.App.Init(this);
		}

		static Assembly LoadFromLibFolder(object sender, ResolveEventArgs args)
		{
			//var assemblyPath = Path.GetFullPath(Path.Combine(unmanagedLibsFolder, new AssemblyName(args.Name).Name + ".dll"));
			//return File.Exists(assemblyPath) ? Assembly.LoadFrom(assemblyPath) : null;
			var name = new AssemblyName(args.Name);
			if (!name.Name.Contains("Localization.resources"))
			{
				if (!name.Name.Contains("Localization")) return null;
				var libPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Localization", name.Name + ".dll");
				return File.Exists(libPath) ? Assembly.LoadFrom(libPath) : null;
			}
			var reg = name.CultureInfo.Name;

			var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Localization", reg, name.Name + ".dll");
			return File.Exists(path) ? Assembly.LoadFrom(path) : null;
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
			;
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
