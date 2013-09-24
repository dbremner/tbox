using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common.Data;
using ScriptEngine.Core.Interfaces;

namespace ScriptEngine.Core
{
	public class Compiler : ICompiler
	{
		private readonly static CodeDomProvider Provider = CodeDomProvider.CreateProvider("CSharp");
		private readonly IDictionary<string, IList<string>> knownReferences;
		private readonly static Regex Regex = new Regex("using (?<assembly>.{1,});", RegexOptions.Compiled);
		private const int MaxCacheSize = 16;
		private static readonly IList<Pair<int, CompilerResults>> CachedResults = 
			new List<Pair<int, CompilerResults>>();
		private static readonly object Locker = new object();
		public Compiler(IDictionary<string, IList<string>> knownReferences)
		{
			this.knownReferences = knownReferences;
		}

		public void Execute(string sourceText)
		{
			DoOperation(sourceText, Execute);
		}

		public void Build(string sourceText)
		{
			DoOperation(sourceText, x=>Build(x));
		}

		protected void DoOperation(string sourceText, Action<CompilerResults> action)
		{
			lock (Locker)
			{
				var results = GetResults(sourceText);
				CheckErrors(results);
				action(results);
			}
		}

		private CompilerResults GetResults(string sourceText)
		{
			var hash = sourceText.GetHashCode();
			var cache = CachedResults.FirstOrDefault(x => x.Key == hash);
			if (cache != null)
			{
				return cache.Value;
			}
			var results = Provider.CompileAssemblyFromSource(CreateParameters(sourceText), sourceText);
			if (!results.Errors.HasErrors)
			{
				CachedResults.Add(new Pair<int, CompilerResults>(hash, results));
				if (CachedResults.Count > MaxCacheSize)
				{
					CachedResults.RemoveAt(0);
				}
			}
			return results;
		}

		private CompilerParameters CreateParameters(string sourceText)
		{
			var parameters = new CompilerParameters
				{
					GenerateExecutable = false,
					GenerateInMemory = true,
					IncludeDebugInformation = false,
					CompilerOptions = "/optimize"
				};
			if (string.IsNullOrEmpty(sourceText)) return parameters;
			var set = new HashSet<string>();
			foreach (var key in sourceText
				.Split(new[]{Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
				.Select(line => Regex.Match(line))
				.Where(m => m.Success)
				.Select(m => m.Groups[1].Value.Trim())
				)
			{
				if (set.Contains(key))continue;
				set.Add(key);
				if (!knownReferences.ContainsKey(key))continue;
				foreach (var path in knownReferences[key])
				{
					var fileName = Path.GetFileName(path);
					if (string.IsNullOrEmpty(fileName) || set.Contains(fileName)) continue;
					set.Add(fileName);
					parameters.ReferencedAssemblies.Add(path);
				}
			}
			Parallel.For(0, parameters.ReferencedAssemblies.Count,
				i => Assembly.ReflectionOnlyLoadFrom(parameters.ReferencedAssemblies[i]));
			return parameters;
		}

		private void Execute(CompilerResults results)
		{
			Execute(Build(results));
		}

		protected virtual object Build(CompilerResults results)
		{
			var types = results.CompiledAssembly.GetExportedTypes();
			if (types.Length != 1) throw new ArgumentException("Module should contains only one public class!");
			var targetType = types.First().FullName;
			var o = results.CompiledAssembly.CreateInstance(targetType);
			if (o == null) throw new ArgumentException("Can't crete instance of class: " + targetType);
			return o;
		}

		protected virtual void Execute(object o)
		{
			var type = o.GetType();
			var m = type.GetMethod("Main");
			if (m == null) throw new ArgumentException("Class should contains only method Main!");
			m.Invoke(o, null);
		}

		private static void CheckErrors(CompilerResults results)
		{
			if (results.Errors.Count <= 0) return;
			var ex = new CompilerExceptions();
			foreach (CompilerError err in results.Errors)
			{
				ex.Errors.Add(err);
			}
			throw ex;
		}

	}
}
