using LightInject;
using Mnk.Library.Common.Tools;
using Mnk.Library.ScriptEngine.Core;
using Mnk.Library.ScriptEngine.Core.Interfaces;
using Mnk.TBox.Core.Contracts;
using Mnk.TBox.Plugins.SkyNet.Code.Interfaces;
using Mnk.TBox.Tools.SkyNet.Common;

namespace Mnk.TBox.Plugins.SkyNet.Code
{
    static class ServicesRegistrator
    {
        public static IServiceContainer Register(IPluginContext context)
        {
            var container = new ServiceContainer();
            container.Register<IScriptCompiler<ISkyScript>, ScriptCompiler<ISkyScript>>(new PerContainerLifetime());
            container.Register<ICopyDirGenerator, CopyDirGenerator>();
            container.Register<IDataPacker, DataPacker>();
            container.Register<ITaskExecutor, TaskExecutor>();
            container.Register<IConfigsFacade, ConfigsFacade>();
            container.Register<IServicesFacade, ServicesFacade>();
            container.Register<ISettingsLogic, SettingsLogic>();
            container.RegisterInstance(context);
            
            return container;
        }
    }
}
