using System;
using System.Collections.Generic;
using System.Linq;
using Mnk.Library.ScriptEngine.Core.Interfaces;
using Mnk.Library.ScriptEngine.Core.Params;
using ScriptEngine.Core.Params;

namespace Mnk.Library.ScriptEngine.Core
{
	public class ScriptCompiler<T> : Compiler<T>, IScriptCompiler<T>
        where T : class
	{
		private readonly Factory factory = new Factory();

        protected override T FindInterface(object o)
		{
			var script = o as T;
			if (script == null) throw new ArgumentException(
				  string.Format("Invalid object type: {0}, should be inherited from: {1}",
				  o.GetType().FullName, typeof(T).FullName));
			return script;
		}

		public T Compile(string sourceText, IEnumerable<Parameter> parameters)
		{
		    var script = Compile(sourceText);
            foreach (var param in parameters)
            {
                var p = script.GetType().GetProperty(param.Key);
                if (p != null) p.SetValue(script, param.GetValue(), null);
            }
		    return script;
		}

        public ScriptPackage GetPackages(string sourceText)
		{
			var parameters = new List<Parameter>();
			var o = Compile(sourceText);
			foreach (var p in o.GetType().GetProperties())
			{
                try
                {
                    var attribute = p.GetCustomAttributes(true).OfType<ParameterAttribute>().SingleOrDefault();
                    if(attribute is IgnoreAttribute)continue;
                    var el = factory.Create(attribute, p.Name, p.PropertyType);
                    parameters.Add(el);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Can't init parameter: " + p.Name, ex);
                }
			}
            return new ScriptPackage { Parameters = parameters, SourceText = sourceText};
		}

	}
}
