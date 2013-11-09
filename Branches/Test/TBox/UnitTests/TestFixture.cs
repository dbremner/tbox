using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Common.SaveLoad;
using NUnit.Framework;
using ScriptEngine.Core;

namespace UnitTests
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
