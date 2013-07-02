using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Common.Base;
using Common.Base.Log;
using Common.Data;
using Common.Tools;
using Interface;
using TBox.Code;
using TBox.Code.ErrorsSender;
using TBox.Code.FastStart;
using TBox.Code.Menu;
using TBox.Forms;
using WPFControls.Code.Log;
using WPFControls.Code.OS;
using WPFControls.Dialogs;
using WPFControls.Dialogs.StateSaver;
using WPFWinForms;
using IUpdater = Common.MT.IUpdater;

namespace TBox
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public sealed partial class MainWindow
	{
		private static readonly ILog Log = LogManager.GetLogger<MainWindow>();
		private static readonly ILog InfoLog = LogManager.GetInfoLogger<MainWindow>();
		private static readonly string LogsFolder = Path.Combine(Environment.CurrentDirectory, "Logs");
		private static readonly string ErrorsLogsPath = Path.Combine(LogsFolder,  "errors.log");
		private const string Caption = "Settings";
		private readonly LogsSender logsSender;
		private readonly PluginsSettings settings;
		private readonly FastStartDialog fastStartDialog;
		private readonly MenuItemsProvider menuItemsProvider;
		private readonly UiConfigurator uiConfigurator;
		private readonly MenuCallsVisitor menuCallsVisitor;
		private Engine engine;
		private ChangesLogDialog changesLogDialog;
		private readonly int startTime = Environment.TickCount;

		public MainWindow()
		{
			if (!OneInstance.MainWindow.Init(this)) return;
			if (!Directory.Exists(LogsFolder)) Directory.CreateDirectory(LogsFolder);
			LogManager.Init( new MultiLog(new IBaseLog[]{
					new FileLog(ErrorsLogsPath), new MsgLog()
				}),
				new FileLog(Path.Combine(LogsFolder, "info.log")));
			logsSender = new LogsSender(ErrorsLogsPath);
			InitializeComponent();
			menuItemsProvider = new MenuItemsProvider();
			menuCallsVisitor = new MenuCallsVisitor();
			settings = new PluginsSettings(menuItemsProvider);
			fastStartDialog = new FastStartDialog{Icon = Icon};
			InfoLog.Write("Init main form time: {0}", Environment.TickCount - startTime);
			uiConfigurator = new UiConfigurator(
				PluginsNames, PluginsBack, settings, menuItemsProvider, menuCallsVisitor, fastStartDialog,
				new[]
					{
						new USeparator(),
						new UMenuItem{Header = Caption + "...", OnClick = o=>MenuShowSettings(), Icon = Properties.Resources.Icon},
						new UMenuItem{Header = "Fast Start...", OnClick = o=>MenuShowFastStart(), Icon = Properties.Resources.Icon},
						new UMenuItem{Header = "Check updates", OnClick = CheckUpdates},
						new UMenuItem{Header = "Exit", OnClick = o=>MenuClose()}
					},
				new[] { new Pair<string, Control>(Caption, settings) },
				OnPluginChanged
				);
			ShowProgress( "Initialize...", CreateEngine, false );
			if (!engine.IsInitialized) return;
			InitFastMenu();
			ExceptionsHelper.HandleException(ShowChangeLog, () => "Error processing changelog", Log);
			this.SetState(engine.Config.DialogState);
		    Show();
			Hide();
			if (engine.Config.StartHidden) return;
			uiConfigurator.FastStartShower.Show();
		}

		private void InitFastMenu()
		{
			var recentItemsCollector = new RecentItemsCollector(engine.Config.FastStartConfig.MenuItems, menuItemsProvider);
			menuCallsVisitor.ClearHandlers();
			menuCallsVisitor.AddHandler(recentItemsCollector);
			uiConfigurator.FastStartShower.Load(engine.Config.FastStartConfig);
			fastStartDialog.Init(recentItemsCollector, engine.Config.FastStartConfig, uiConfigurator.FastStartShower, menuItemsProvider);
		}

		private void CheckUpdates(object o)
		{
			engine.CheckUpdates(o is SchedulerContext);
		}

		private void ShowChangeLog()
		{
			var cfg = engine.Config.Update;
			if (!cfg.ShowChanglog ) return;
			var file = new FileInfo("changelog.txt");
			if (!file.Exists) return;
			if (file.Length <= cfg.LastChanglogPosition + 10) return;
			changesLogDialog = new ChangesLogDialog();
			changesLogDialog.ShowDialog(file.ReadBegin((int)(file.Length - cfg.LastChanglogPosition)));
			cfg.LastChanglogPosition = file.Length;
		}

		private void OnPluginChanged(string name)
		{
			if (string.Equals(name, Caption))
			{
				Title = Caption;
			}
			else
			{
				Title = Caption + " - [" + name + "]";
			}
		}

		private void ShowProgress( string caption, Action<IUpdater> action, bool withOwner = true)
		{
			DialogsCache.ShowProgress(action, caption, withOwner?this:null);
		}

		public void MenuClose(bool criticalError = false )
		{
			if (changesLogDialog != null)
			{
				changesLogDialog.Dispose();
			}
			CanClose = true;
			uiConfigurator.Dispose();
			if (engine != null)
			{
				Mt.Do(this, () =>
					{
						engine.Config.DialogState = this.GetState();
						fastStartDialog.Dispose();
					});
				ShowProgress("Exit...", u => engine.Close(u, criticalError), false);
				if (engine.Config.ErrorReports.AllowSend)
				{
					logsSender.SendIfNeed(engine.Config.ErrorReports.Directory);
				}
				Mt.Do(this, () => {
						engine.Dispose();
						DialogsCache.Dispose();
					});
			}
			Close();
		}

		private void CreateEngine(IUpdater updater)
		{
			updater.Update( "Loading plugins...", 0 );
			engine = new Engine(this, updater, uiConfigurator);
			updater.Update( "Finish...", 1 );
			InfoLog.Write("Create engine time: {0}", Environment.TickCount - startTime);
		}

		private void MenuShowSettings()
		{
			uiConfigurator.FastStartShower.ShowSettings();
		}

		private void MenuShowFastStart()
		{
			uiConfigurator.FastStartShower.ShowFastStart();
		}

		private void BtnSaveClick( object sender, RoutedEventArgs e )
		{
			ShowProgress( "Apply changes...", u => engine.Save(u, false));
			if (engine.Config.HideOnSave) Close();
		}

		private void BtnReloadClick( object sender, RoutedEventArgs e )
		{
			ShowProgress("Reload plugins settins...", u =>
				{
					engine.Load(u);
					InitFastMenu();
				});
			if (engine.Config.HideOnCancel) Close();
		}

		private void BtnCloseClick( object sender, RoutedEventArgs e )
		{
			Close();
		}

		private void BtnFastStartClick(object sender, RoutedEventArgs e)
		{
			uiConfigurator.FastStartShower.ShowFastStart();
			Close();
		}
	}
}
