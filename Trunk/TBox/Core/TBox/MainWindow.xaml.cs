using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using Common.Base;
using Common.Base.Log;
using Common.Tools;
using Interface;
using Localization.TBox;
using TBox.Code;
using TBox.Code.ErrorsSender;
using TBox.Code.FastStart;
using TBox.Code.Menu;
using TBox.Code.Objects;
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
		private static readonly string LogsFolder = Path.Combine(Folders.UserFolder, "Logs");
		private static readonly string ErrorsLogsPath = Path.Combine(LogsFolder,  "errors.log");
		private readonly LogsSender logsSender;
		private readonly PluginsSettings settings;
		private readonly Lazy<FastStartDialog> fastStartDialog;
		private readonly MenuItemsProvider menuItemsProvider;
		private readonly UiConfigurator uiConfigurator;
		private readonly MenuCallsVisitor menuCallsVisitor;
		private readonly ConfigManager configManager;
		private Engine engine;
		private ChangesLogDialog changesLogDialog;
		private readonly int startTime = Environment.TickCount;
		internal RecentItemsCollector RecentItemsCollector;
		private readonly FeedbackSender feedbackSender = new FeedbackSender();

		public MainWindow()
		{
			RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
			if (!OneInstance.MainWindow.Init(this)) return;
			if (!Directory.Exists(LogsFolder)) Directory.CreateDirectory(LogsFolder);
			LogManager.Init( new MultiLog(new IBaseLog[]{
					new FileLog(ErrorsLogsPath), new MsgLog()
				}),
				new FileLog(Path.Combine(LogsFolder, "info.log")));
			logsSender = new LogsSender(ErrorsLogsPath);
			configManager = new ConfigManager();
			InitializeComponent();
			menuItemsProvider = new MenuItemsProvider();
			menuCallsVisitor = new MenuCallsVisitor();
			RecentItemsCollector = new RecentItemsCollector(configManager, menuItemsProvider);
			settings = new PluginsSettings(menuItemsProvider);
			fastStartDialog = new Lazy<FastStartDialog>(CreateFastStartDialog);
			InfoLog.Write("Init main form time: {0}", Environment.TickCount - startTime);
			uiConfigurator = new UiConfigurator(
				View, PluginsBack, BtnBack, settings, menuItemsProvider, menuCallsVisitor,
				new[]
					{
						new USeparator(),
						new UMenuItem{Header = TBoxLang.UserActions}, 
						new USeparator(),
						new UMenuItem{Header = TBoxLang.MenuSettings, OnClick = o=>MenuShowSettings(), Icon = Properties.Resources.Icon},
						new UMenuItem{Header = TBoxLang.MenuFastStart, OnClick = o=>MenuShowFastStart(), Icon = Properties.Resources.Icon},
						new UMenuItem{Header = TBoxLang.MenuCheckUpdates, OnClick = CheckUpdates},
						new UMenuItem{Header = TBoxLang.MenuExit, OnClick = o=>MenuClose()}
					},
				new[]
				{
					new EngineSettingsInfo(TBoxLang.SettingsCaption, TBoxLang.SettingsDescription, Properties.Resources.Icon, () => settings),
					new EngineSettingsInfo(TBoxLang.FastStart, TBoxLang.FastStartDescription, Properties.Resources.Icon,
						() =>
						{
							configManager.Config.FastStartConfig.IsFastStart = true;
							return fastStartDialog.Value;
						})
				},
				OnPluginChanged
				);
			ShowProgress( TBoxLang.ProgressInitialize, CreateEngine, false );
			InitFastStartMenu();

			ExceptionsHelper.HandleException(ShowChangeLog, () => TBoxLang.ErrorProcessingChangelog, Log);
			this.SetState(configManager.Config.DialogState);
		    Show();
			Hide();
			if (configManager.Config.StartHidden) return;
			uiConfigurator.FastStartShower.Show();
		}

		private void InitFastStartMenu()
		{
			uiConfigurator.FastStartShower.Load(configManager.Config.FastStartConfig);
			menuCallsVisitor.ClearHandlers();
			menuCallsVisitor.AddHandler(RecentItemsCollector);
			if (fastStartDialog.IsValueCreated)
			{
				fastStartDialog.Value.Init(configManager, RecentItemsCollector);
			}
		}

		private FastStartDialog CreateFastStartDialog()
		{
			var dialog = new FastStartDialog(
				() =>
				{
					BackClick(null, null);
					Close();
				});
			dialog.Init(configManager, RecentItemsCollector);
			return dialog;
		}

		private void CheckUpdates(object o)
		{
			engine.CheckUpdates(o is NonUserRunContext);
		}

		private void ShowChangeLog()
		{
			var cfg = configManager.Config.Update;
			if (!cfg.ShowChanglog ) return;
			var file = new FileInfo("changelog.txt");
			if (!file.Exists) return;
			if (file.Length <= cfg.LastChanglogPosition + 10) return;
			changesLogDialog = new ChangesLogDialog();
			changesLogDialog.ShowDialog(file.ReadBegin((int)(file.Length - cfg.LastChanglogPosition)));
			cfg.LastChanglogPosition = file.Length;
		}

		private void OnPluginChanged(EnginePluginInfo info)
		{
			if (string.Equals(info.Name, TBoxLang.Caption))
			{
				Title = TBoxLang.Caption;
			}
			else
			{
				Title = TBoxLang.Caption + " - [" + info.Name + "]";
			}
			Description.Text = info.Description;
		}

		private void ShowProgress( string caption, Action<IUpdater> action, bool withOwner = true)
		{
			DialogsCache.ShowProgress(action, caption, withOwner?this:null, topmost:false, icon: Icon);
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
						configManager.Config.DialogState = this.GetState();
					});
				ShowProgress(TBoxLang.ProgressExit, u => engine.Close(u, criticalError), false);
				if (configManager.Config.ErrorReports.AllowSend)
				{
					logsSender.SendIfNeed(configManager.Config.ErrorReports.Directory);
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
			updater.Update( TBoxLang.ProgressLoadingPlugins, 0 );
			engine = new Engine(this, updater, uiConfigurator, configManager);
			updater.Update( TBoxLang.ProgressFinish, 1 );
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
			ShowProgress( TBoxLang.ProgressApplyChanges, u => engine.Save(u, false));
			if (configManager.Config.HideOnSave) Close();
		}

		private void BtnReloadClick( object sender, RoutedEventArgs e )
		{
			ShowProgress(TBoxLang.ProgressReloadPluginsSettings, u =>
				{
					engine.Load(u);
					InitFastStartMenu();
				});
			if (configManager.Config.HideOnCancel) Close();
		}

		private void SendFeedback(object sender, RoutedEventArgs e)
		{
			var r = DialogsCache.ShowMemoBox(TBoxLang.FeedbackMessage, TBoxLang.FeedbackCaption, engine.ConfigManager.Config.FeedBackMessage, x => !string.IsNullOrEmpty(x), this);
			if (!r.Key) return;
			configManager.Config.FeedBackMessage = r.Value;
			DialogsCache.ShowProgress(u =>
			{
				if (!feedbackSender.Send("feedback", configManager.Config.FeedBackMessage))
				{
					Dispatcher.BeginInvoke(new Action(()=>Feedback.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent))));
				}
			}, TBoxLang.FeedbackProgress, this, false);
		}

		public void GoBack()
		{
			PluginsBack.Content = View;
			Description.Text = TBoxLang.DefaultMainViewDescription;
			Title = TBoxLang.Caption;
			BtnBack.IsEnabled = false;
		}

		private void BackClick(object sender, RoutedEventArgs e)
		{
			GoBack();
            configManager.Config.FastStartConfig.IsFastStart = false;
		}

		private void BtnHelpClick(object sender, RoutedEventArgs e)
		{
			using (Process.Start("https://tbox.codeplex.com")) { }
		}

		private void BtnCloseClick(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
