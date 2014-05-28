using LightInject;
using Mnk.Library.Common.Tools;
using Mnk.Library.ScriptEngine.Core;
using Mnk.Library.ScriptEngine.Core.Interfaces;
using Mnk.TBox.Plugins.SkyNet.Code.Interfaces;
using Mnk.TBox.Tools.SkyNet.Common;

namespace Mnk.TBox.Plugins.SkyNet.Code
{
    class ServicesRegistrator
    {
        public IServiceContainer Register()
        {
            var container = new ServiceContainer();
            container.Register<IScriptCompiler<ISkyScript>, ScriptCompiler<ISkyScript>>(new PerContainerLifetime());
            container.Register<ICopyDirGenerator, CopyDirGenerator>();
            container.Register<IDataPacker, DataPacker>();
            container.Register<ITaskExecutor, TaskExecutor>();
            container.Register<IServicesBuilder, ServicesBuilder>();
            container.Register<IConfigsFacade, ConfigsFacade>();
            
            return container;
        }
    }
}
