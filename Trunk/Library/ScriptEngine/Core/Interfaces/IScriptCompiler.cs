using System.Collections.Generic;
using Mnk.Library.ScriptEngine.Core.Params;
using ScriptEngine.Core.Params;

namespace Mnk.Library.ScriptEngine.Core.Interfaces
{
	public interface IScriptCompiler<out T> : ICompiler<T>
	{
		T Compile(string sourceText, IEnumerable<Parameter> parameters);
        ScriptPackage GetPackages(string sourceText);
	}
}
