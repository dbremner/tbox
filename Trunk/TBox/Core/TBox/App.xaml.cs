using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Mnk.Library.Common;
using Mnk.Library.Common.Log;
using Mnk.TBox.Locales.Localization.TBox;
using Mnk.Library.WpfControls.Code.OS;
using Mnk.Library.WpfControls.Localization;
using Mnk.Library.WpfWinForms;

namespace Mnk.TBox.Core.Application
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private readonly ILog Log = LogManager.GetLogger<App>();
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

			ShutdownMode = ShutdownMode.OnMainWindowClose;
			FormsStyles.Enable();
			AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
			DispatcherUnhandledException += CurrentDispatcherUnhandledException;
			Dispatcher.UnhandledException += DispatcherOnUnhandledException;
			TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

			OneInstance.App.Init(this);
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
