using System.Collections.Generic;
using System.Reflection;

namespace ScriptEngine.Core.Assemblies
{
	class AssemblyInfo
	{
		public Assembly Assembly { get; set; }
		public IList<string> References { get; set; }
	}
}
