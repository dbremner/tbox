using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Base;
using Common.Base.Log;
using Interface;
using Interface.Atrributes;
using PluginsShared.Tools;
using ProjectMan.Code;
using WPFWinForms;

namespace ProjectMan
{
	[PluginName("Project manager")]
	[PluginDescription("Easy way to manage your projects. You can rebuild, work with svn\n and run it's scripts from tray.")]
	public class ProjectMan : ConfigurablePlugin<Settings, Config>
	{
		private static readonly ILog Log = LogManager.GetLogger<ProjectMan>();
		private readonly GroupOperations groupOperations = new GroupOperations();
		private readonly SvnStatisticOperations svnStatisticOperations = new SvnStatisticOperations();
		public override void OnRebuildMenu()
		{
			base.OnRebuildMenu();
			var projectContext = CreateContext();
			var items = Config.Dirs.CheckedItems.ToArray();
			var list = new List<KeyValuePair<string, UMenuItem>>();
			foreach (var dir in items)
			{
				CreateNewProject(list, dir.Key, projectContext);
			}
			var pathes = items.Select(x => x.Key).ToArray();
			var menu = list.OrderBy(x => x.Key).Select(x => x.Value).ToList();
			groupOperations.Append(menu, projectContext, Context, pathes);
			svnStatisticOperations.Append(menu, projectContext, Context, Config.SvnUserName);
			Menu = menu.ToArray();
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			Icon = context.GetSystemIcon(43);
		}

		private ProjectContext CreateContext()
		{
			return new ProjectContext(
				new SvnProvider(Config.PathToSvn), 
				new MsBuildProvider(Config.PathToMsBuild, Context.DataProvider.ToolsPath)
				);
		}

		private void CreateNewProject(ICollection<KeyValuePair<string, UMenuItem>> list, string path, ProjectContext context)
		{
			if (!Directory.Exists(path))
			{
				Log.Write("Path not exist: '{0}'", path);
				return;
			}
			list.Add(new KeyValuePair<string, UMenuItem>(
							 path,
							 (new Project(path, context, Context)).MenuItem
							 ));
		}
	}
}
