using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mnk.Library.Common.Log;
using Mnk.TBox.Core.Interface;
using Mnk.TBox.Core.Interface.Atrributes;
using Mnk.TBox.Locales.Localization.Plugins.ProjectMan;
using Mnk.TBox.Core.PluginsShared.Tools;
using Mnk.TBox.Plugins.ProjectMan.Code;
using Mnk.TBox.Plugins.ProjectMan.Code.Settings;
using Mnk.Library.WpfWinForms;
using Mnk.Library.WpfWinForms.Icons;

namespace Mnk.TBox.Plugins.ProjectMan
{
	[PluginInfo(typeof(ProjectManLang), 43, PluginGroup.Development)]
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
				CreateNewProject(list, dir, projectContext);
			}
			var paths = items.Select(x => x.Key).ToArray();
			var menu = list.OrderBy(x => x.Key).Select(x => x.Value).ToList();
			groupOperations.Append(menu, projectContext, Context, paths, Icon.ToImageSource());
			svnStatisticOperations.Append(menu, projectContext, Context, Config.SvnUserName);
			Menu = menu.ToArray();
		}

		private ProjectContext CreateContext()
		{
			return new ProjectContext(
				new SvnProvider(Config.PathToSvn), 
				new MsBuildProvider(Config.PathToMsBuild, Context.DataProvider.ToolsPath)
				);
		}

		private void CreateNewProject(ICollection<KeyValuePair<string, UMenuItem>> list, ProjectInfo info, ProjectContext context)
		{
			if (!Directory.Exists(info.Key))
			{
				Log.Write("Path not exist: '{0}'", info.Key);
				return;
			}
			list.Add(new KeyValuePair<string, UMenuItem>(
							 info.Key,
							 (new Project(info, context, Context)).MenuItem
							 ));
		}
	}
}
