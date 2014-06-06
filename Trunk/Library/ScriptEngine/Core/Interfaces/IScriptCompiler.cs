using System.Collections.Generic;
using ScriptEngine.Core.Params;

namespace Mnk.Library.ScriptEngine.Core.Interfaces
{
    public interface IScriptCompiler<out T> : ICompiler<T>
    {
        T Compile(string sourceText, IEnumerable<Parameter> parameters);
        ScriptPackage GetPackages(string sourceText);
    }
}
