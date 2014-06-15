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
        private static readonly string TestsDllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Mnk.TBox.Tests.dll");
        private const string ToolsPath = "../../../bin/" + Shared.CompileMode + "/Tools";
        private static readonly string NUnitAgentPath = Path.Combine(ToolsPath, "NUnitAgent.exe");
        private static readonly string RunAsx86Path = Path.Combine(ToolsPath, "RunAsx86.exe");
        public static readonly bool[] Bools = {true,false};
        public static readonly string[] Frameworks = { "net-4.0" };
        private IServiceContainer container;
        private TestsConfig config;
        private ITestsUpdater updater;
        private ITestsFixture testsFixture;

        [SetUp]
        public void SetUp()
        {
            config = new TestsConfig
            {
                NunitAgentPath = NUnitAgentPath,
                RunAsx86Path = RunAsx86Path,
                DirToCloneTests = Path.GetTempPath(),
                RunAsAdmin = false,
                Mode = TestsRunnerMode.Process
            };
            updater = new SimpleUpdater(new ConsoleUpdater());
            container = ServicesRegistrar.Register();
            testsFixture = container.GetInstance<ITestsFixture>();

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
            Assert.IsFalse(testsFixture.EnsurePathIsValid(config));
        }

        [Test]
        public void When_check_valid_path([ValueSource("Bools")]bool x86)
        {
            //Arrange
            config.TestDllPath = TestsDllPath;
            config.RunAsx86 = x86;

            //Assert
            Assert.IsTrue(testsFixture.EnsurePathIsValid(config));
        }

        [Test]
        public void When_calc_tests_should_be_no_erros([ValueSource("Bools")]bool x86)
        {
            //Arrange
            config.TestDllPath = TestsDllPath;
            config.RunAsx86 = x86;
            testsFixture.EnsurePathIsValid(config);

            //Act
            var results = testsFixture.Refresh(config);

            //Assert
            Assert.IsFalse(results.IsFailed);
        }

        [Test]
        public void When_calc_tests_should_calc_them([ValueSource("Bools")]bool x86)
        {
            //Arrange
            config.TestDllPath = TestsDllPath;
            config.RunAsx86 = x86;
            testsFixture.EnsurePathIsValid(config);

            //Act
            var results = testsFixture.Refresh(config);

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

            testsFixture.EnsurePathIsValid(config);
            var results = testsFixture.Refresh(config);

            //Act
            results = testsFixture.Run(config, results, updater);

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
