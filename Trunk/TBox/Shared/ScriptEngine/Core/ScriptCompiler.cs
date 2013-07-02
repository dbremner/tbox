using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using ScriptEngine.Core.Interfaces;
using ScriptEngine.Core.Params;

namespace ScriptEngine.Core
{
	public class ScriptCompiler : Compiler, IScriptCompiler
	{
		private readonly Factory factory = new Factory();
		private readonly IScriptRunner scriptRunner = new ScriptRunner();
		public static IScriptContext Sc { get; private set; }
		public ScriptCompiler(IScriptContext scriptContext)
		{
			Sc = scriptContext;
		}

		public ScriptCompiler()
		{
			
		}

		protected override void  Execute(object o)
		{
			scriptRunner.Run((IScript)o);
		}

		protected override object Build(CompilerResults results)
		{
			var o = base.Build(results);
			var script = o as IScript;
			if (script == null) throw new ArgumentException(
				  string.Format("Invalid object type: {0}, should be inherited from: {1}",
				  o.GetType().FullName, typeof(IScript).FullName));
			return o;
		}

		public void Execute(string sourceText, IEnumerable<Parameter> parameters, Action<Action> dispatcher)
		{
			DoOperation(sourceText, o => dispatcher(()=>DoExecute(o, parameters)));
		}

		private void DoExecute(CompilerResults results, IEnumerable<Parameter> parameters)
		{
			var o = Build(results);
			foreach (var param in parameters)
			{
				var p = o.GetType().GetProperty(param.Key);
				if(p!=null)p.SetValue(o, param.GetValue(), null);
			}
			Execute(o);
		}

		public ExecutionContext GetExecutionContext(string sourceText, Action<Action> dispatcher)
		{
			ExecutionContext executionContext = null;
			DoOperation(sourceText, o =>
				{
					executionContext = new ExecutionContext(p => dispatcher(()=>DoExecute(o, p)), GetParams(o));
				});
			return executionContext;
		}

		protected IList<Parameter> GetParams(CompilerResults results)
		{
			var parameters = new List<Parameter>();
			var o = Build(results);
			foreach (var p in o.GetType().GetProperties())
			{
				var attribute = p.GetCustomAttributes(true).OfType<ParameterAttribute>().SingleOrDefault();
				var el = factory.Create(attribute, p.Name, p.PropertyType);
				if(el!=null)parameters.Add(el);
			}
			return parameters;
		}

	}
}
