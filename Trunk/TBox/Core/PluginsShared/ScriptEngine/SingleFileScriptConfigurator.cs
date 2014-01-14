using System.Collections.Generic;
using System.IO;
using Mnk.Library.ScriptEngine;

namespace Mnk.TBox.Core.PluginsShared.ScriptEngine
{
    public class SingleFileScriptConfigurator : ScriptsConfigurator
    {
        protected override IEnumerable<string> GetPathes()
        {
            return new[] { Path.Combine(Context.DataProvider.ReadOnlyDataPath, ((SingleFileOperation)Config).Path) };
        }
    }
}
