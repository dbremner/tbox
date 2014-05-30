using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mnk.Library.Common.MT;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.TBox.Tests.Common;
using NUnit.Core;
using NUnit.Framework;
using Mnk.Library.ParallelNUnit.Infrastructure;
using Mnk.Library.ParallelNUnit.Infrastructure.Interfaces;
using Mnk.Library.ParallelNUnit.Infrastructure.Packages;
using Mnk.Library.ParallelNUnit.Infrastructure.Updater;
using Rhino.Mocks;

namespace Mnk.TBox.Tests.PlugingsShared.UnitTestsRunner
{
    [TestFixture]
    [Category("Integration")]
    class When_running_unit_test
    {
	    private TestsMetricsCalculator calculator = null;
        private static readonly string TestsDllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../Library/Library.Tests/bin/" + Shared.CompileMode + "/", "Mnk.Library.Tests.dll");
        private const string ToolsPath = "../../../bin/" + Shared.CompileMode + "/Tools";
        private static readonly string NUnitAgentPath = Path.Combine(ToolsPath, "NUnitAgent.exe");
        private static readonly string RunAsx86Path = Path.Combine(ToolsPath, "RunAsx86.exe");
        public static readonly bool[] Bools = {true,false};
        public static readonly string[] Frameworks = { "net-4.0" };

        [SetUp]
        public void SetUp()
        {
            calculator = null;
        }

        [Test]
        public void When_check_invalid_path([ValueSource("Bools")]bool x86)
        {
            //Arrange
            var view = MockRepository.GenerateMock<IUnitTestsView>();
            using (var p = new ProcessPackage("Wrong path", NUnitAgentPath, x86, false, Path.GetTempPath(), string.Empty, view, RunAsx86Path, null))
            {
                //Assert
                Assert.IsFalse(p.EnsurePathIsValid());
            }
        }

        [Test]
        public void When_check_valid_path([ValueSource("Bools")]bool x86)
        {
            //Arrange
            var view = MockRepository.GenerateMock<IUnitTestsView>();
            using (var p = new ProcessPackage(TestsDllPath, NUnitAgentPath, x86, false, Path.GetTempPath(), string.Empty, view, RunAsx86Path, null))
            {
                //Assert
                Assert.IsTrue(p.EnsurePathIsValid());
            }
        }

        [Test]
        public void When_calc_tests_should_be_no_erros([ValueSource("Bools")]bool x86)
        {
            //Arrange
            var view = MockRepository.GenerateMock<IUnitTestsView>();
            using (var p = new ProcessPackage(TestsDllPath, NUnitAgentPath, x86, false, Path.GetTempPath(), string.Empty, view, RunAsx86Path, null))
            {
                p.EnsurePathIsValid();

                //Act
                var error = false;
                p.DoRefresh(x => { }, x => error = true);

                //Assert
                Assert.IsFalse(error);
            }
        }

        [Test]
        public void When_calc_tests_should_calc_them([ValueSource("Bools")]bool x86)
        {
            //Arrange
            var view = MockRepository.GenerateMock<IUnitTestsView>();
            using (
                var p = new ProcessPackage(TestsDllPath, NUnitAgentPath, x86, false, Path.GetTempPath(), string.Empty,
                                         view, RunAsx86Path, null))
            {
                p.EnsurePathIsValid();

                //Act
                var count = 0;
                p.DoRefresh(x => { count = x.Count; }, x => { });

                //Assert
                Assert.Greater(count, 20);
            }
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
            var view = MockRepository.GenerateMock<IUnitTestsView>();
			view.Stub(x => x.SetItems(Arg<IList<Result>>.Is.Anything, 
                Arg<TestsMetricsCalculator>.Matches(m => CheckCalc(m))));

            using (var p = new ProcessPackage(TestsDllPath, NUnitAgentPath, x86, false, Path.GetTempPath(), string.Empty, view, RunAsx86Path, framework))
            {
                p.EnsurePathIsValid();

                p.DoRefresh(x => { }, x => {});

                var packages = p.PrepareToRun(ncores, new[]{"Integration"}, false, prefetch);
                var updater = new ConsoleUpdater();
                var synchronizer = new Synchronizer(ncores);

                //Act
                p.DoRun(x => x.ApplyResults(prefetch), p.Items, packages, copy, new []{"*.dll;*.exe"}, sync, startDelay, synchronizer, new SimpleUpdater(updater, synchronizer), needOutput);
                Assert.Greater(p.Count, 20);
                Assert.AreEqual(0, p.FailedCount, CollectFailed());
            }
        }
		
        private bool CheckCalc(TestsMetricsCalculator testsMetricsCalculator)
        {
            calculator = testsMetricsCalculator;
            return true;
        }

        private string CollectFailed()
        {
            return string.Join(Environment.NewLine,
                calculator.All
                    .Where(x => x.IsTest && x.Executed &&
                            (x.State == ResultState.Error || x.State == ResultState.Failure))
                    .Select(x => x.Key + " ------ " + x.Message + " ------ " + x.StackTrace)
                );
        }

    }
}
