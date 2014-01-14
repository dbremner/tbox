using Mnk.TBox.Core.PluginsShared.ReportsGenerator;
using Mnk.TBox.Core.PluginsShared.ScriptEngine;
using Mnk.Library.ScriptEngine.Core;
using Mnk.Library.ScriptEngine.Core.Interfaces;

namespace Mnk.TBox.Tools.SkyNet.Common.Scripts
{
    public class SkyNetScriptConfigurator : IScriptConfigurator
    {
        private readonly IScriptCompiler<IReportScript> compiler = new ScriptCompiler<IReportScript>();

        public ScriptPackage GetParameters(string scriptText)
        {
            return compiler.GetPackages(scriptText);
        }
    }
}
