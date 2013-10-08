using System;
using System.Collections.Generic;
using System.IO;
using Automator.Code.Settings;
using ScriptEngine.Core;
using ScriptEngine.Core.Assemblies;
using ScriptEngine.Core.Params;

namespace Automator.Code
{
	sealed class Runner : IRunner, IDisposable
	{
		private static readonly object Locker = new object();
		private static AssembliesCollector collector;
		private readonly Config config;

		public Runner(Config config)
		{
			this.config = config;
		}

		public void Dispose()
		{
			
		}

		public void Build(string text)
		{
			DoOperation(c => c.Build(text));
		}

		public void Run(string path, string rootPath, ICollection<Parameter> parameters, Action<Action> dispatcher)
		{
			var undoPath = Path.Combine(rootPath, "Undo");
			var context = new ScriptContext(undoPath);
			DoOperation(c=>c.Execute(File.ReadAllText(path), parameters, dispatcher), context);
		}

		public ExecutionContext GetExecutionContext(string path, Action<Action> dispatcher)
		{
			ExecutionContext context = null;
			DoOperation(c => context = c.GetExecutionContext(File.ReadAllText(path), dispatcher));
			return context;
		}

		private void DoOperation(Action<ScriptCompiler> a, ScriptContext context = null)
		{
			var op = new Action(() => a(new ScriptCompiler(config.KnownReferences, context)));
			lock (Locker)
			{
				try
				{
					op();
				}
				catch (Exception ex)
				{
					if (collector != null || 
						(!(ex is CompilerExceptions) && !(ex is FileNotFoundException)))
						throw;
					RecollectLibs(op);
				}
			}
		}

		private void RecollectLibs(Action op)
		{
			collector = new AssembliesCollector();
			config.KnownReferences = collector.Collect();
			op();
		}
	}
}
