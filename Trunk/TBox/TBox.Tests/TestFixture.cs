using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Mnk.Library.Common.SaveLoad;
using NUnit.Framework;
using Mnk.Library.ScriptEngine.Core;

namespace Mnk.TBox.Tests
{
    [SetUpFixture]
    public class TestFixture
    {
        [SetUp]
        public void RunBeforeAnyTests()
        {
            CompilerCore.Serializer = new ParamSerializer<IDictionary<string, IList<string>>>(Path.GetTempFileName());
            foreach (var path in Directory.GetFiles(Environment.CurrentDirectory, "*.dll"))
            {
                Assembly.LoadFile(path);
            }
        }
    }
}
