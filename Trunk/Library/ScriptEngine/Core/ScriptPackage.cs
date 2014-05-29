using System.Collections.Generic;
using ScriptEngine.Core.Params;

namespace Mnk.Library.ScriptEngine.Core
{
    public class ScriptPackage
    {
        public IList<Parameter> Parameters { get; set; }
        public string SourceText { get; set; }
    }
}
