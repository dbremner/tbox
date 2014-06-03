using System;
using System.IO;
using System.Linq;
using LightInject;
using Mnk.Library.Common.MT;
using Mnk.Library.ParallelNUnit;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.TBox.Tests.Common;
using NUnit.Core;
using NUnit.Framework;
using Rhino.Mocks;

namespace Mnk.TBox.Tests.PlugingsShared.ParallelNunit
{
    [TestFixture]
    [Category("Integration")]
    class When_running_unit_test_in_process
    {
        private static readonly string TestsDllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../Library/Library.Tests/bin/" + Shared.CompileMode + "/", "Mnk.Library.Tests.dll");
        private const string ToolsPath = "../../../bin/" + Shared.CompileMode + "/Tools";
        private static readonly string NUnitAgentPath = Path.Combine(ToolsPath, "NUnitAgent.exe");
        private static readonly string RunAsx86Path = Path.Combine(ToolsPath, "RunAsx86.exe");
        public static readonly bool[] Bools = {true,false};
        public static readonly string[] Frameworks = { "net-4.0" };
        private IServiceContainer container;
        private ProcessTestConfig config;
        private ITestsView view;
        private ITestsUpdater updater;
        private IPackage<IProcessTestConfig> package;

        [SetUp]
        public void SetUp()
        {
            config = new ProcessTestConfig
            {
                NunitAgentPath = NUnitAgentPath,
                RunAsx86Path = RunAsx86Path,
                DirToCloneTests = Path.GetTempPath(),
                RunAsAdmin = false,
            };
            view = MockRepository.GenerateStub<ITestsView>();
            updater = new SimpleUpdater(new ConsoleUpdater());
            container = ServicesRegistrar.Register();
            package = container.GetInstance<IPackage<IProcessTestConfig>>();

        }

        [TearDown]
        public void TearDown()
        {
            container.Dispose();
        }

        [Test]
        public void When_check_invalid_path([ValueSource("Bools")]bool x86)
        {
            //Arrange
            config.TestDllPath = "Wrong path";
            config.RunAsx86 = x86;

            //Assert
            Assert.IsFalse(package.EnsurePathIsValid(config));
        }

        [Test]
        public void When_check_valid_path([ValueSource("Bools")]bool x86)
        {
            //Arrange
            config.TestDllPath = TestsDllPath;
            config.RunAsx86 = x86;

            //Assert
            Assert.IsTrue(package.EnsurePathIsValid(config));
        }

        [Test]
        public void When_calc_tests_should_be_no_erros([ValueSource("Bools")]bool x86)
        {
            //Arrange
            config.TestDllPath = TestsDllPath;
            config.RunAsx86 = x86;
            package.EnsurePathIsValid(config);

            //Act
            var results = package.Refresh(config);

            //Assert
            Assert.IsFalse(results.IsFailed);
        }

        [Test]
        public void When_calc_tests_should_calc_them([ValueSource("Bools")]bool x86)
        {
            //Arrange
            config.TestDllPath = TestsDllPath;
            config.RunAsx86 = x86;
            package.EnsurePathIsValid(config);

            //Act
            var results = package.Refresh(config);

            //Assert
            Assert.Greater(results.Metrics.Total, 20);
        }

        [Test]
        [Pairwise]
        public void When_run_tests(
            [ValueSource("Bools")]bool x86, 
            [Values(1, 2)]int ncores, 
            [ValueSource("Bools")]bool sync, 
            [ValueSource("Bools")]bool copy,
            [ValueSource("Bools")]bool needOutput,
            [ValueSource("Bools")]bool prefetch,
            [ValueSource("Frameworks")]string framework,
            [Values(0, 1)]int startDelay)
        {
            //Arrange
            config.TestDllPath = TestsDllPath;
            config.RunAsx86 = x86;
            config.RuntimeFramework = framework;
            config.ProcessCount = ncores;
            config.Categories = new[] {"Integration"};
            config.IncludeCategories = false;
            config.OptimizeOrder = prefetch;
            config.CopyToSeparateFolders = copy;
            config.CopyMasks = new[] {"*.dll;*.exe"};
            config.StartDelay = startDelay;
            config.NeedSynchronizationForTests = sync;
            config.NeedOutput = needOutput;

            package.EnsurePathIsValid(config);
            var results = package.Refresh(config);

            //Act
            results = package.Run(config, results, updater);

            //Assert
            Assert.Greater(results.Metrics.Total, 20);
            Assert.AreEqual(0, results.Metrics.FailedCount, CollectFailed(results));
        }

        private string CollectFailed(TestsResults results)
        {
            return string.Join(Environment.NewLine,
                results.Metrics.All
                    .Where(x => x.IsTest && x.Executed &&
                            (x.State == ResultState.Error || x.State == ResultState.Failure))
                    .Select(x => x.Key + " ------ " + x.Message + " ------ " + x.StackTrace)
                );
        }

    }
}
