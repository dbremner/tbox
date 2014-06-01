using LightInject;
using Mnk.Library.Common.Tools;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Packages;
using Mnk.Library.ParallelNUnit.Packages.Common;
using Mnk.Library.ParallelNUnit.Packages.Excecution;

namespace Mnk.Library.ParallelNUnit
{
    public static class ServicesRegistrator
    {
        public static IServiceContainer Register(IThreadTestConfig config, ITestsView view, ITestsUpdater updater)
        {
            return RegisterComponents(config, view, updater);
        }

        public static IServiceContainer Register(IProcessTestConfig config, ITestsView view, ITestsUpdater updater)
        {
            return RegisterComponents(config, view, updater);
        }

        private static IServiceContainer RegisterComponents<TConfig>(TConfig config, ITestsView view, ITestsUpdater updater)
            where TConfig: ITestsConfig
        {
            var container = new ServiceContainer();

            container.RegisterInstance(config);
            container.RegisterInstance((ITestsConfig)config);
            container.RegisterInstance(view);
            container.RegisterInstance(updater);


            container.Register<ICopyDirGenerator, CopyDirGenerator>(new PerContainerLifetime());
            container.Register<ITestsMetricsCalculator, TestsMetricsCalculator>(new PerContainerLifetime());
            container.Register<IDirectoriesManipulator, DirectoriesManipulator>(new PerContainerLifetime());
            container.Register<IPrefetchManager, PrefetchManager>(new PerContainerLifetime());
            container.Register<ISynchronizer, Synchronizer>(new PerContainerLifetime());

            container.Register<IProcessCreator, ProcessCreator>(new PerContainerLifetime());
            container.Register<IProcessCalculator, ProcessCalculator>(new PerContainerLifetime());
            container.Register<IProcessTestsRunner, ProcessTestsRunner>(new PerContainerLifetime());

            container.Register<IThreadTestsRunner, ThreadTestsRunner>(new PerContainerLifetime());
            container.Register<IThreadTestsExecutor, ThreadTestsExecutor>(new PerContainerLifetime());

            container.Register<IPackage<IThreadTestConfig>, ThreadPackage>(new PerContainerLifetime());
            container.Register<IPackage<IProcessTestConfig>, ProcessPackage>(new PerContainerLifetime());

            return container;
        }
    }
}
