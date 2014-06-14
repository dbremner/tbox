using System;
using LightInject;
using Mnk.Library.Common.Tools;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit.Packages;
using Mnk.Library.ParallelNUnit.Packages.Common;
using Mnk.Library.ParallelNUnit.Packages.Excecution;

namespace Mnk.Library.ParallelNUnit
{
    public static class ServicesRegistrar
    {
        public static void Register(IServiceContainer container)
        {
            container.Register<INUnitTestFacade, NUnitTestFacade>(new PerContainerLifetime());
            container.Register<ICopyDirGenerator, CopyDirGenerator>(new PerContainerLifetime());
            container.Register<ITestsDivider, TestsDivider>(new PerContainerLifetime());
            container.Register<ITestsExecutor, TestsExecutor>(new PerContainerLifetime());
            container.Register<ITestsMetricsCalculator, TestsMetricsCalculator>(new PerContainerLifetime());
            container.Register<IDirectoriesManipulator, DirectoriesManipulator>(new PerContainerLifetime());
            container.Register<IOrderOptimizationManager, OrderOptimizationManager>(new PerContainerLifetime());

            container.Register<ITestsExecutionFacade, ThreadTestsExecutionFacade>(TestsRunnerType.Thread, new PerContainerLifetime());
            container.Register<ITestsExecutionFacade, ProcessTestsExecutionFacade>(TestsRunnerType.Process, new PerContainerLifetime());
            
            container.RegisterInstance(typeof(Func<string, ITestsExecutionFacade>), 
                new Func<string, ITestsExecutionFacade>( container.GetInstance<ITestsExecutionFacade>));

            container.Register<ITestsFixture, TestsFixture>(new PerContainerLifetime());
        }

        public static IServiceContainer Register()
        {
            var container = new ServiceContainer();

            Register(container);

            return container;
        }
    }
}
