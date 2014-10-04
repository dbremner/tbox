using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Mnk.Library.Common;
using Mnk.Library.Common.Log;
using Mnk.Library.WpfControls;
using Mnk.Library.WpfControls.Localization;
using Mnk.Library.WpfWinForms;

namespace Mnk.TBox.Core.Application
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private readonly ILog log = LogManager.GetLogger<App>();
		private static bool handled = false;

		public App()
		{
			Translator.Culture = new CultureInfo("en");

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
				log.Write((Exception)ex, message);
			}
			else log.Write(message);
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
