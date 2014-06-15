using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mnk.Library.Common.Log;

namespace Mnk.Library.ScriptEngine.Core.Assemblies
{
	public class AssembliesCollector
	{
		private static readonly ILog InfoLog = LogManager.GetInfoLogger<AssembliesCollector>();
		public IDictionary<string, IList<string>> Collect()
		{
			var time = Environment.TickCount;
			var result = new ConcurrentDictionary<string, IList<string>>();
			var knownLibs = new HashSet<string>();
			Parallel.ForEach(
				GetAssemblies(GetAssemblies().Where(x => !x.IsDynamic), knownLibs), 
				info => AddAssembly(result, info));
			InfoLog.Write("Collect assemblies time: {0}", Environment.TickCount - time);
			return result.ToDictionary(x=>x.Key, x=>x.Value);
		}

	    private static IEnumerable<Assembly> GetAssemblies()
	    {
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                yield return a;
            }
	        var path = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(Path.Combine(path, "Libraries")))
	        {
	            path = Path.GetFullPath(path + "/..");
	        }
	        if (Directory.Exists(Path.Combine(path, "Libraries")))
	        {
                foreach (var dir in new[] { "Libraries", "Localization" })
                {
                    foreach (var file in Directory.EnumerateFiles(Path.Combine(path, dir), "*.dll"))
                    {
                        Assembly a;
                        try
                        {
                            a = Assembly.LoadFile(file);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        yield return a;
                    }
                }
	        }
	    }

	    private static IEnumerable<AssemblyInfo> GetAssemblies(IEnumerable<Assembly> assemblies, ISet<string> knownLibs)
		{
			foreach (var asm in assemblies)
			{
				lock (knownLibs)
				{
					if (knownLibs.Contains(asm.Location)) continue;
					knownLibs.Add(asm.Location);
				}
				var references = asm.GetReferencedAssemblies().Select(a => Assembly.Load(a.FullName)).ToArray();
				yield return new AssemblyInfo
					{
						Assembly = asm,
						References = references.Select(x=>x.Location).ToArray()
					};
				foreach (var referenced in GetAssemblies(references, knownLibs))
				{
					yield return referenced;
				}
			}
		}

		private static void AddAssembly(IDictionary<string, IList<string>> result, AssemblyInfo info)
		{
            foreach (var nsp in info.Assembly.GetExportedTypes()
                .Where(x => x.Namespace!=null)
                .Select(x => x.Namespace)
                .Distinct())
			{
				result[nsp] = (new[] { info.Assembly.Location }.Concat(info.References)).ToArray();
			}
		}
	}
}
