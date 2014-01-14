using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mnk.Library.ScriptEngine;

namespace Mnk.TBox.Core.PluginsShared.ScriptEngine
{
    public class MultiFileScriptConfigurator : ScriptsConfigurator
    {
        protected override IEnumerable<string> GetPathes()
        {
            return ((MultiFileOperation)Config).Pathes
                .CheckedItems
                .Select(x => Path.Combine(Context.DataProvider.ReadOnlyDataPath, x.Key))
                .ToArray();
        }
    }
}
