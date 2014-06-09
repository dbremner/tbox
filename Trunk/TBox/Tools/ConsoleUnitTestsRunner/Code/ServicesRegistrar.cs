using LightInject;
using Mnk.Library.Common.MT;
using Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code.Contracts;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code
{
    internal static class ServicesRegistrar
    {
        public static IServiceContainer Register()
        {
            var container = new ServiceContainer();
            Library.ParallelNUnit.ServicesRegistrar.Register(container);
            container.Register<IInfoView, InfoView>(new PerContainerLifetime());
            container.Register<IUpdater, ConsoleUpdater>(new PerContainerLifetime());
            container.Register<IReportBuilder, ReportBuilder>(new PerContainerLifetime());
            container.Register<ITestsExecutor, TestsExecutor>(new PerContainerLifetime());
            container.Register<IExecutor, Executor>(new PerContainerLifetime());
            return container;
        }
    }
}
