using LightInject;
using Mnk.Library.Common.MT;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code.Contracts;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code
{
    internal static class ServicesRegistrar
    {
        public static IServiceContainer Register()
        {
            var container = new ServiceContainer();
            container.Register<IConsoleView, ConsoleView>(new PerContainerLifetime());
            container.Register<ITestsSummaryBuilder, TestsSummaryBuilder>(new PerContainerLifetime());
            container.Register<IInfoView, InfoView>(new PerContainerLifetime());
            container.Register<IUpdater, ConsoleUpdater>(new PerContainerLifetime());
            container.Register<IReportBuilder, ReportBuilder>(new PerContainerLifetime());
            container.Register<ITestsExecutor, TestsExecutor>(new PerContainerLifetime());
            container.Register<IExecutor, Executor>(new PerContainerLifetime());
            return container;
        }
    }
}
