using System;
using System.Linq;
using System.Reflection;
using Mnk.Library.ScriptEngine.Core.Interfaces;

namespace Mnk.Library.ScriptEngine.Core
{
	public class Compiler<T> : ICompiler<T>
        where T : class
	{
	    private readonly CompilerCore core = new CompilerCore();

        public T Compile(string sourceText)
        {
            var results = core.Build(sourceText);
            return Compile(results);
        }

		private T Compile(Assembly assembly)
		{
            var types = assembly.GetExportedTypes();
            if (types.Length != 1) throw new ArgumentException("Module should contains only one public class!");
            var targetType = types.First().FullName;
            var o = assembly.CreateInstance(targetType);
            if (o == null) throw new ArgumentException("Can't create instance of class: " + targetType);
		    return FindInterface(o);
		}

		protected virtual T FindInterface(object o)
		{
			var type = o.GetType();
			var m = type.GetMethod("Main");
			if (m == null) throw new ArgumentException("Class should contains only method Main!");
		    return o as T;
		}
	}
}
