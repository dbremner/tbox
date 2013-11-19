using System.Collections.Generic;
using System.Xml.Linq;
using PluginsShared.Automator;
using ScriptEngine;
using PluginsShared.Tools;

namespace Solution.Scripts
{
    public class AddTracing : IScript
    {
        [FileDictionary("d:/project/Web.config", "d:/sample.log")]
        public IDictionary<string, string> Files { get; set; }

        public void Run(IScriptContext s)
        {
            var i = 0;
            foreach (var file in Files)
            {
                s.Updater.Update(file.Key, i++/(float)Files.Count);
                var doc = XDocument.Load(file.Key, LoadOptions.PreserveWhitespace);
                AddTracingToConfig(doc.Root, file.Value);
                doc.Save(file.Key);
            }
        }

        private static void AddTracingToConfig(XElement root, string targetPath)
        {
            root.AddNodeIfNotExist("system.diagnostics/trace")
                .SetAttributeIfNeed("autoflush", "true");
            root.AddNodeIfNotExist("system.diagnostics/sources/source")
                .SetAttributeIfNeed("name", "System.Net.Sockets")
                .SetAttributeIfNeed("maxdatasize", "10241024");
            root.AddNodeIfNotExist("system.diagnostics/sources/source/listeners/add")
                .SetAttributeIfNeed("name", "TraceFile");
            root.AddNodeIfNotExist("system.diagnostics/sharedListeners/add")
                .SetAttributeIfNeed("name", "TraceFile")
                .SetAttributeIfNeed("type", "System.Diagnostics.TextWriterTraceListener")
                .SetAttributeIfNeed("initializeData", targetPath);
            root.AddNodeIfNotExist("system.diagnostics/switches/add")
                .SetAttributeIfNeed("name", "System.Net.Sockets")
                .SetAttributeIfNeed("value", "Verbose");
        }
    }
}
