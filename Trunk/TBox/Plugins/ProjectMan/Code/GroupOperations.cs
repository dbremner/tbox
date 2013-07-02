using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.MT;
using Interface;
using WPFControls.Dialogs;
using WPFWinForms;

namespace ProjectMan.Code
{
	class GroupOperations
	{
		public void Append(IList<UMenuItem> menu, ProjectContext context, IPluginContext pluginContext, string[] pathes)
		{
			var enabled = menu.Count > 0;
			menu.Add(new USeparator());
			menu.Add(new UMenuItem
			{
				IsEnabled = enabled,
				Header = "Update all",
				Icon = pluginContext.GetIcon(context.SvnProvider.Path, 3),
				OnClick = o=>Run(u => Update(context, pathes, u))
			});
			menu.Add(new UMenuItem
			{
				IsEnabled = enabled,
				Header = "Rebuild all in release",
				Icon = pluginContext.GetIcon(context.MsBuildProvider.PathToMsBuild, 0),
				OnClick = o => Run(u => Rebuild(context, "Release", pathes,u))
			});
			menu.Add(new UMenuItem
			{
				IsEnabled = enabled,
				Header = "Rebuild all in debug",
				Icon = pluginContext.GetIcon(context.MsBuildProvider.PathToMsBuild, 0),
				OnClick = o => Run(u => Rebuild(context, "Debug", pathes,u))
			});
			menu.Add(new UMenuItem
			{
				IsEnabled = enabled,
				Header = "Update and rebuild all in debug",
				Icon = pluginContext.GetIcon(context.MsBuildProvider.PathToMsBuild, 0),
				OnClick = o => Run(u => UpdateAndBuid(context, pathes,u))
			});
		}

		private void UpdateAndBuid(ProjectContext context, IList<string> pathes, IUpdater u)
		{
			Do(x => {
					context.SvnProvider.Do("update", x, true);
					if(u.UserPressClose)return;
					ReBuild(context, "Debug", x);
				}, pathes, u);
		}

		private static void Rebuild(ProjectContext context, string mode, IList<string> pathes, IUpdater u)
		{
			Do(x => ReBuild(context, mode, x), pathes, u);
		}

		private static void Update(ProjectContext context, IList<string> pathes, IUpdater u)
		{
			Do(x => context.SvnProvider.Do("update", x, true), pathes, u);
		}

		private static void Do(Action<string> action, IList<string> pathes, IUpdater u)
		{
			for (var i = 0; i < pathes.Count; i++)
			{
				if (u.UserPressClose) break;
				var path = pathes[i];
				u.Update(path, i / (float)pathes.Count);
				action(path);
			}
		}

		private static void ReBuild(ProjectContext context, string mode, string path)
		{
			var slnFiles = Directory.GetFiles(path, "*.sln");
			if( !slnFiles.Any() )return;
			foreach (var file in slnFiles)
			{
				context.MsBuildProvider.Build(mode, file, true);
			}
		}

		private static void Run(Action<IUpdater> task)
		{
			DialogsCache.ShowProgress(task, "Project manager process projects");
		}
	}
}
