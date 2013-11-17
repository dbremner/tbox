using System;
using System.IO;
using System.Linq;
using Common.MT;
using NUnit.Framework;
using PluginsShared.UnitTests;
using PluginsShared.UnitTests.Interfaces;
using PluginsShared.UnitTests.Updater;
using Rhino.Mocks;

namespace UnitTests.PlugingsShared.UnitTestsRunner
{
    [TestFixture]
    [Category("Integration")]
    class When_running_unit_test
    {
        private static readonly string TestsDllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UnitTests.dll");
        private const string ToolsPath = "../../../bin/Release/Tools";
        private static readonly string NUnitAgentPath = Path.Combine(ToolsPath, "NUnitAgent.exe");
        private static readonly string RunAsx86Path = Path.Combine(ToolsPath, "RunAsx86.exe");
        public static readonly bool[] Bools = new []{true,false};

        [Test]
        public void When_check_invalid_path([ValueSource("Bools")]bool x86)
        {
            //Arrange
            var view = MockRepository.GenerateMock<IUnitTestsView>();
            using (var p = new TestsPackage("Wrong path", NUnitAgentPath, x86, false, Path.GetTempPath(), string.Empty, view, RunAsx86Path))
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
            using (var p = new TestsPackage(TestsDllPath, NUnitAgentPath, x86, false, Path.GetTempPath(), string.Empty, view, RunAsx86Path))
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
            using (var p = new TestsPackage(TestsDllPath, NUnitAgentPath, x86, false, Path.GetTempPath(), string.Empty, view, RunAsx86Path))
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
                var p = new TestsPackage(TestsDllPath, NUnitAgentPath, x86, false, Path.GetTempPath(), string.Empty,
                                         view, RunAsx86Path))
            {
                p.EnsurePathIsValid();

                //Act
                var count = 0;
                p.DoRefresh(x => { count = x.Count; }, x => { });

                //Assert
                Assert.Greater(count, 210);
            }
        }

        [Test]
        [Pairwise]
        public void When_run_tests([ValueSource("Bools")]bool x86, [Values(1, 2)]int ncores, [ValueSource("Bools")]bool sync, [ValueSource("Bools")]bool copy)
        {
            //Arrange
            var view = MockRepository.GenerateMock<IUnitTestsView>();
            using (var p = new TestsPackage(TestsDllPath, NUnitAgentPath, x86, false, Path.GetTempPath(), string.Empty, view, RunAsx86Path))
            {
                p.EnsurePathIsValid();

                p.DoRefresh(x => { }, x => {});

                var packages = p.PrepareToRun(ncores, new[]{"Integration"}, false);
                var updater = new ConsoleUpdater();
                var synchronizer = new Synchronizer(ncores);

                //Act
                p.DoRun(x => x.ApplyResults(), packages, copy, 1, sync, synchronizer, new SimpleUpdater(updater, synchronizer));
                Assert.Greater(p.Count, 210);
                Assert.AreEqual(0, p.FailedCount);
            }
        }
    }
}
