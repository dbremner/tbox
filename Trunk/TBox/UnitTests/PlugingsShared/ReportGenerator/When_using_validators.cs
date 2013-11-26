using System.IO;
using NUnit.Framework;
using PluginsShared.ReportsGenerator;
using ScriptEngine.Core;

namespace UnitTests.PlugingsShared.ReportGenerator
{
    [TestFixture]
    [Category("Integration")]
    class When_using_validators
    {
        public static string[] Files
        {
            get { return Directory.GetFiles("../../../bin/Release/Data/TeamManager/Validators/", "*.cs", SearchOption.TopDirectoryOnly); }
        }

        [Test]
        public void Should_get_packages_for_all_exist_scripts([ValueSource("Files")] string path)
        {
            //Arrange
            var sc = new ScriptCompiler<IDayStatusStrategy>();

            //Act & Assert
            Assert.IsNotNull(sc.GetPackages(File.ReadAllText(path)));
        }

        [Test]
        public void Should_compile_all_exist_scripts([ValueSource("Files")] string path)
        {
            //Arrange
            var sc = new ScriptCompiler<IDayStatusStrategy>();

            //Act & Assert
            Assert.IsNotNull(sc.Compile(File.ReadAllText(path)));
        }
    }
}
