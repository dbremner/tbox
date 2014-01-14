using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Mnk.TBox.Core.PluginsShared.Automator;
using Mnk.Library.ScriptEngine.Core;
using Mnk.Library.ScriptEngine.Core.Params;
using ScriptEngine.Core.Params;

namespace Mnk.TBox.Tests.PlugingsShared.Automator
{
    [TestFixture]
    [Category("Integration")]
    class When_using_script_runner
    {
        public static string[] Files
        {
            get { return Directory.GetFiles("../../../bin/Release/Data/Automater/Scripts/", "*.cs", SearchOption.TopDirectoryOnly); }
        }

        [Test]
        public void Should_get_packages_for_all_exist_scripts([ValueSource("Files")] string path)
        {
            //Arrange
            var sc = new ScriptCompiler<IScript>();

            //Act & Assert
            Assert.IsNotNull(sc.GetPackages(File.ReadAllText(path)));
        }

        [Test]
        public void Should_compile_all_exist_scripts([ValueSource("Files")] string path)
        {
            //Arrange
            var sc = new ScriptCompiler<IScript>();

            //Act & Assert
            Assert.IsNotNull(sc.Compile(File.ReadAllText(path), new List<Parameter>()));
        }
    }
}
