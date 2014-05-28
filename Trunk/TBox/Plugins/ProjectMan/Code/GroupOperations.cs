using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using Mnk.Library.Common.MT;
using Mnk.TBox.Core.Interface;
using Mnk.TBox.Locales.Localization.Plugins.ProjectMan;
using Mnk.Library.WpfControls.Dialogs;
using Mnk.Library.WpfWinForms;

namespace Mnk.TBox.Plugins.ProjectMan.Code
{
    class GroupOperations
    {
        public void Append(IList<UMenuItem> menu, ProjectContext context, IPluginContext pluginContext, string[] paths, ImageSource icon)
        {
            var enabled = menu.Count > 0;
            menu.Add(new USeparator());
            menu.Add(new UMenuItem
            {
                IsEnabled = enabled,
                Header = ProjectManLang.UpdateAll,
                Icon = pluginContext.GetIcon(context.SvnProvider.Path, 3),
                OnClick = o => Run(u => Update(context, paths, u), icon)
            });
            menu.Add(new UMenuItem
            {
                IsEnabled = enabled,
                Header = ProjectManLang.RebuildAllInRelease,
                Icon = pluginContext.GetIcon(context.MsBuildProvider.PathToMsBuild, 0),
                OnClick = o => Run(u => Rebuild(context, "Release", paths, u), icon)
            });
            menu.Add(new UMenuItem
            {
                IsEnabled = enabled,
                Header = ProjectManLang.RebuildAllInDebug,
                Icon = pluginContext.GetIcon(context.MsBuildProvider.PathToMsBuild, 0),
                OnClick = o => Run(u => Rebuild(context, "Debug", paths, u), icon)
            });
            menu.Add(new UMenuItem
            {
                IsEnabled = enabled,
                Header = ProjectManLang.UpdateAndRebuildAllInDebug,
                Icon = pluginContext.GetIcon(context.MsBuildProvider.PathToMsBuild, 0),
                OnClick = o => Run(u => UpdateAndBuid(context, paths, u), icon)
            });
        }

        private void UpdateAndBuid(ProjectContext context, IList<string> paths, IUpdater u)
        {
            Do(x =>
            {
                context.SvnProvider.Do("update", x, true);
                if (u.UserPressClose) return;
                ReBuild(context, "Debug", x);
            }, paths, u);
        }

        private static void Rebuild(ProjectContext context, string mode, IList<string> paths, IUpdater u)
        {
            Do(x => ReBuild(context, mode, x), paths, u);
        }

        private static void Update(ProjectContext context, IList<string> paths, IUpdater u)
        {
            Do(x => context.SvnProvider.Do("update", x, true), paths, u);
        }

        private static void Do(Action<string> action, IList<string> paths, IUpdater u)
        {
            for (var i = 0; i < paths.Count; i++)
            {
                if (u.UserPressClose) break;
                var path = paths[i];
                u.Update(path, i / (float)paths.Count);
                action(path);
            }
        }

        private static void ReBuild(ProjectContext context, string mode, string path)
        {
            var slnFiles = Directory.GetFiles(path, "*.sln");
            if (!slnFiles.Any()) return;
            foreach (var file in slnFiles)
            {
                context.MsBuildProvider.Build(mode, file, true);
            }
        }

        private static void Run(Action<IUpdater> task, ImageSource icon)
        {
            DialogsCache.ShowProgress(task, ProjectManLang.ProjectManagerProcessProjects, null, icon: icon, showInTaskBar: true);
        }
    }
}
