using LightInject;
using Mnk.Library.Common.MT;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code.Contracts;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code
{
    internal static class ServicesRegistrar
    {
        public static IServiceContainer Register()
        {
            var container = new ServiceContainer();
            Library.ParallelNUnit.ServicesRegistrar.Register(container);
            container.Register<IInfoView, InfoView>();
            container.Register<IUpdater, ConsoleUpdater>();
            container.Register<IReportBuilder, ReportBuilder>();
            container.Register<ITestsExecutor, TestsExecutor>();
            container.Register<ITestsView, ConsoleView>();
            container.Register<IExecutor, Executor>();
            return container;
        }
    }
}
