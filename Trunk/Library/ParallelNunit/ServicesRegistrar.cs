using LightInject;
using Mnk.Library.Common.Tools;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Packages;
using Mnk.Library.ParallelNUnit.Packages.Common;
using Mnk.Library.ParallelNUnit.Packages.Excecution;

namespace Mnk.Library.ParallelNUnit
{
    public static class ServicesRegistrar
    {
        public static IServiceContainer Register()
        {
            var container = new ServiceContainer();

            container.Register<ICopyDirGenerator, CopyDirGenerator>(new PerContainerLifetime());
            container.Register<ITestsMetricsCalculator, TestsMetricsCalculator>(new PerContainerLifetime());
            container.Register<IDirectoriesManipulator, DirectoriesManipulator>(new PerContainerLifetime());
            container.Register<IOrderOptimizationManager, OrderOptimizationManager>(new PerContainerLifetime());
            container.Register<IThreadTestsExecutor, ThreadTestsExecutor>(new PerContainerLifetime());

            container.Register<ITestsRunner<IThreadTestConfig>, ThreadTestsRunner>(new PerContainerLifetime());
            container.Register<ITestsRunner<IProcessTestConfig>, ProcessTestsRunner>(new PerContainerLifetime());

            container.Register<IPackage<IThreadTestConfig>, ThreadPackage>(new PerContainerLifetime());
            container.Register<IPackage<IProcessTestConfig>, ProcessPackage>(new PerContainerLifetime());

            return container;
        }
    }
}
