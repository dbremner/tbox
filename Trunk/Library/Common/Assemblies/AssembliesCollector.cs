using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Common.Assemblies
{
	public class AssembliesCollector
	{
		private class AssemblyInfo
		{
			public Assembly Assembly { get; set; }
			public IList<string> References { get; set; }
		}
		public IDictionary<string, IList<string>> Collect()
		{
			var result = new ConcurrentDictionary<string, IList<string>>();
			var knownLibs = new HashSet<string>();
			Parallel.ForEach(
				GetAssemblies(AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic), knownLibs), 
				info => AddAssembly(result, info));
			return result;
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
				var references = asm.GetReferencedAssemblies().Select(a => Assembly.ReflectionOnlyLoad(a.FullName)).ToArray();
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
			foreach (var nsp in info.Assembly.GetExportedTypes().Select(x => x.Namespace).Distinct())
			{
				result[nsp] = (new[] { info.Assembly.Location }.Concat(info.References)).ToArray();
			}
		}
	}
}
