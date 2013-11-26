using System.IO;
using Common.AutoUpdate;
using Common.Base.Log;
using Interface;
using LightInject;
using TBox.Code.AutoUpdate;
using TBox.Code.Configs;
using TBox.Code.ErrorsSender;
using TBox.Code.FastStart;
using TBox.Code.Managers;
using TBox.Code.Menu;
using TBox.Code.Objects;
using TBox.Forms;
using WPFControls.Code.Log;
using WPFWinForms.Icons;

namespace TBox.Code
{
    class ServicesRegistrator
    {
        private static readonly string LogsFolder = Path.Combine(Folders.UserRootFolder, "Logs");
        private static readonly string ErrorsLogsPath = Path.Combine(LogsFolder, "errors.log");

        public ServicesRegistrator()
        {
            if (!Directory.Exists(LogsFolder)) Directory.CreateDirectory(LogsFolder);
            LogManager.Init(new MultiLog(new IBaseLog[]{
					new FileLog(ErrorsLogsPath), new MsgLog()
				}),
                new FileLog(Path.Combine(LogsFolder, "info.log")));
        }

        public IServiceContainer Register()
        {
            var container = new ServiceContainer();
            container.Register(f=>new LogsSender(ErrorsLogsPath), new SingletonLifetime());
            var cm = new ConfigManager();
            container.Register<IConfigManager<Config>>(f=>cm, new SingletonLifetime());
            container.Register<IConfigsManager>(f=>cm, new SingletonLifetime());
            container.Register<IMenuItemsProvider, MenuItemsProvider>(new SingletonLifetime());
            container.Register<MenuCallsVisitor>(new SingletonLifetime());
            container.Register<RecentItemsCollector>(new SingletonLifetime());
            container.Register<InfoDialog>(new SingletonLifetime());
            container.Register<PluginsSettings>(new SingletonLifetime());

            container.Register<IconsCache>(new SingletonLifetime());
            container.Register<IconsExtractor>(new SingletonLifetime());
            container.Register<WarmingUpManager>(new SingletonLifetime());
            container.Register<PluginsContextShared>(new SingletonLifetime());

            container.Register<IApplicationUpdater, CodePlexApplicationUpdater>(new SingletonLifetime());
            container.Register<IAutoUpdater, ApplicationUpdater>(new SingletonLifetime());

            return container;
        }
    }
}
