using System.IO;
using Mnk.Library.Common.AutoUpdate;
using Mnk.Library.Common.Log;
using Mnk.TBox.Core.Interface;
using LightInject;
using Mnk.TBox.Core.Application.Code.AutoUpdate;
using Mnk.TBox.Core.Application.Code.Configs;
using Mnk.TBox.Core.Application.Code.ErrorsSender;
using Mnk.TBox.Core.Application.Code.FastStart;
using Mnk.TBox.Core.Application.Code.Managers;
using Mnk.TBox.Core.Application.Code.Menu;
using Mnk.TBox.Core.Application.Code.Objects;
using Mnk.TBox.Core.Application.Forms;
using Mnk.Library.WpfControls.Code.Log;
using Mnk.Library.WpfWinForms.Icons;

namespace Mnk.TBox.Core.Application.Code
{
    class ServicesRegistrator
    {
        private static readonly string LogsFolder = Path.Combine(Folders.UserRootFolder, "Logs");
        private static readonly string ErrorsLogsPath = Path.Combine(LogsFolder, "TBox.Errors.log");

        public ServicesRegistrator()
        {
            if (!Directory.Exists(LogsFolder)) Directory.CreateDirectory(LogsFolder);
            LogManager.Init(new MultiplyLog(new IBaseLog[]{
					new FileLog(ErrorsLogsPath), new MsgLog()
				}),
                new FileLog(Path.Combine(LogsFolder, "TBox.Info.log")));
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
