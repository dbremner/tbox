using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mnk.Library.Common.Models;
using Mnk.Library.Common.SaveLoad;
using Mnk.Library.ScriptEngine.Core.Assemblies;

namespace Mnk.Library.ScriptEngine.Core
{
    internal class CompilerCore
    {
        private readonly static CodeDomProvider Provider = CodeDomProvider.CreateProvider("CSharp");
        private static IDictionary<string, IList<string>> knownReferences;
        private readonly static Regex Regex = new Regex("using (?<assembly>.{1,});", RegexOptions.Compiled);
        private const int MaxCacheSize = 16;
        private static AssembliesCollector collector;
        private static readonly IList<Pair<int, CompilerResults>> CachedResults =
            new List<Pair<int, CompilerResults>>();
        private static readonly object Locker = new object();

        internal static ConfigurationSerializer<IDictionary<string, IList<string>>> Serializer { get; set; }

        static CompilerCore()
        {
            Serializer = new ConfigurationSerializer<IDictionary<string, IList<string>>>(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TBox.ScriptCompiler.cache"));
            knownReferences = Serializer.Load(new Dictionary<string, IList<string>>());
        }

        public Assembly Build(string sourceText)
        {
            lock (Locker)
            {
                try
                {
                    return DoBuild(sourceText).CompiledAssembly;
                }
                catch (Exception ex)
                {
                    if (!(ex is CompilerExceptions) && !(ex is FileNotFoundException))
                        throw;
                    RecollectLibs();
                    return DoBuild(sourceText).CompiledAssembly;
                }
            }
        }

        private static void RecollectLibs()
        {
            if (collector == null) collector = new AssembliesCollector();
            knownReferences = collector.Collect();
            Serializer.Save(knownReferences);
        }

        private static CompilerResults DoBuild(string sourceText)
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
                return results;
            }
			var ex = new CompilerExceptions();
			foreach (CompilerError err in results.Errors)
			{
				ex.Errors.Add(err);
			}
			throw ex;
        }

        private static CompilerParameters CreateParameters(string sourceText)
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
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => Regex.Match(line))
                .Where(m => m.Success)
                .Select(m => m.Groups[1].Value.Trim())
                )
            {
                if (set.Contains(key)) continue;
                set.Add(key);
                if (!knownReferences.ContainsKey(key)) continue;
                foreach (var path in knownReferences[key])
                {
                    var fileName = Path.GetFileName(path);
                    if (string.IsNullOrEmpty(fileName) || set.Contains(fileName)) continue;
                    set.Add(fileName);
                    if (!File.Exists(path)) throw new FileNotFoundException(path);
                    parameters.ReferencedAssemblies.Add(path);
                }
            }
            Parallel.For(0, parameters.ReferencedAssemblies.Count,
                i => LoadAssembly(parameters, i));
            return parameters;
        }

        private static Assembly LoadAssembly(CompilerParameters parameters, int i)
        {
            try
            {
                return Assembly.ReflectionOnlyLoadFrom(parameters.ReferencedAssemblies[i]);
            }
            catch
            {
                return null;
            }
        }
    }
}
