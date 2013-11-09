using System;
using System.Collections.Generic;
using Common.MT;
using Localization.PluginsShared;
using PluginsShared.ScriptEngine;
using ScriptEngine.Core;
using ScriptEngine.Core.Params;
using WPFControls.Code.OS;
using WPFControls.Dialogs;

namespace PluginsShared.Automator
{
    public class ScriptsRunner : ScriptsConfigurator
    {
        public ScriptsRunner()
        {
            btnAction.Content = PluginsSharedLang.Run;
        }

        protected override void DoAction()
        {
            base.DoAction();
            DialogsCache.ShowProgress(u => Work(u, GetParameters()),
                PluginsSharedLang.Execute + ":" + Title, null, topmost: false, showInTaskBar: true);
        }

        private IList<Parameter> GetParameters()
        {
            return Config.Parameters;
        }

        private void Work(IUpdater u, IList<Parameter> parameters)
        {
            try
            {
                DoWork(u, parameters);
            }
            catch (CompilerExceptions cex)
            {
                Log.Write(cex.ToString());
            }
            catch (Exception ex)
            {
                Log.Write(ex, "Error running script");
            }
        }

        private void DoWork(IUpdater u, IList<Parameter> parameters)
        {
            var i = 0;
            var compiler = new ScriptCompiler<IScript>();
            var ctx = new ScriptContext{Updater = u,Sync = a=>Mt.Do(this, a)};
            foreach (var context in ScriptsPackages)
            {
                if (u.UserPressClose) return;
                u.Update(i++ / (float)ScriptsPackages.Count);
                var s = compiler.Compile(context.SourceText, parameters);
                s.Run(ctx);
            }
        }
    }
}
