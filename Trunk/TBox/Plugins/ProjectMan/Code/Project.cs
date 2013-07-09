using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Common.Base.Log;
using Common.Console;
using Common.Data;
using Interface;
using ProjectMan.Code.Settings;
using WPFControls.Dialogs;
using WPFWinForms;

namespace ProjectMan.Code
{
	sealed class Project
	{
		private static readonly ILog Log = LogManager.GetLogger<Project>();
		private static readonly List<string> KnownArgs = new List<string>(); 
		private readonly DirectoryInfo info;
		private readonly ProjectInfo projectInfo;
		private readonly ProjectContext projectContext;
		private readonly IPluginContext pluginContext;
		public UMenuItem MenuItem { get; private set; }

		public Project(ProjectInfo projectInfo, ProjectContext projectContext, IPluginContext pluginContext)
		{
			info = new DirectoryInfo(projectInfo.Key);
			this.projectInfo = projectInfo;
			this.projectContext = projectContext;
			this.pluginContext = pluginContext;
			CreateMenu();
		}

		private void CreateMenu()
		{
			MenuItem = new UMenuItem{Header = CreateHeader()};
			AppendBuildSubMenu();
			AppendSeparator();
			AppendSvnSubMenu();
			AppendSeparator();
			AppendBatSubMenu();
			AppendMsBuildSubMenu();
		}

		private void AppendSeparator()
		{
			MenuItem.Items.Add(new USeparator());
		}

		private string CreateHeader()
		{
			var header = info.Name;
			if(info.Parent!=null)
			{
				header = info.Parent.Name + "/" + header;
			}
			return header;
		}

		private void AppendItem(string caption, Action<object> onClick, Icon icon = null)
		{
			MenuItem.Items.Add(new UMenuItem
			{
				Header = caption,
				Icon = icon,
				OnClick = onClick
			});
		}

		private void AppendBuildItem(string caption, string path, string mode)
		{
			AppendItem(string.Format("{0} - {1}", caption, mode),
				o => projectContext.MsBuildProvider.Build(mode, path),
				pluginContext.GetIcon(projectContext.MsBuildProvider.PathToMsBuild, 0)
				);
		}

		private void AppendBuildSubMenu()
		{
			var files = info.GetFiles("*.sln", SearchOption.TopDirectoryOnly);
			if (!files.Any()) return;
			foreach (var item in files)
			{
				AppendBuildItem(item.Name, item.FullName, "Debug");
				AppendBuildItem(item.Name, item.FullName, "Release");
			}
		}

		private void AppendSvnItem(string caption, string command, int id)
		{
			AppendItem(caption, o => projectContext.SvnProvider.Do(command, info.FullName), 
				pluginContext.GetIcon(projectContext.SvnProvider.Path, id));
		}

		private void AppendMergeItem(string caption, string command, int id)
		{
			AppendItem(caption, o => projectContext.SvnProvider.Merge(command, info.FullName), 
				pluginContext.GetIcon(projectContext.SvnProvider.Path, id));
		}

		private void AppendSvnSubMenu()
		{
			AppendSvnItem("Update...", "update", 3);
			AppendSvnItem("Commit...", "commit", 4);
			AppendSvnItem("Show log...", "log", 17);
            AppendSeparator();
			AppendSvnItem("Repo Browser...", "repobrowser", 106);
			AppendSvnItem("Resolve...", "resolve", 8);
			AppendSvnItem("Update to revision...", "update /rev", 3);
			AppendSvnItem("Revert...", "revert", 6);
			AppendSvnItem("CleanUp...", "cleanup", 7);
            AppendSeparator();
            AppendSvnItem("Branch/tag...", "copy", 13);
            AppendSvnItem("Switch...", "switch", 9);
            AppendSvnItem("Merge...", "merge", 10);
            AppendSvnItem("Export...", "export", 12);
            AppendSvnItem("Relocate...", "relocate", 22);
            AppendSeparator();
			AppendSvnItem("Create patch...", "createpatch", 109);
			AppendMergeItem("Apply patch...", "patchpath", 108);
			AppendSvnItem("Properties...", "properties", 117);
		}

		private IEnumerable<Pair<string, string>> GetFiles(string ext)
		{
			var files =
				info.GetFiles(ext).
				Select(i => new Pair<string, string>(i.Name, i.FullName));
			foreach (var dir in info.GetDirectories())
			{
				var dirName = dir.Name;
				files = files.Union(
					dir.GetFiles(ext).
					Select(i =>
						new Pair<string, string>(dirName + "\\" + i.Name, i.FullName)));
			}
			return files;
		}

		private void AppendBatSubMenu()
		{
			var files = GetFiles("*.bat").
				Union(GetFiles("*.cmd")).
				OrderBy(x => x.Key);
			if (!files.Any()) return;
			var item = AddNewMenuItem("Batch - *.cmd and *.bat", pluginContext.GetIcon("c:\\windows\\system32\\cmd.exe", 0));
			foreach (var m in files)
			{
				var path = m.Value;
				item.Items.Add(new UMenuItem
					{
						Header = m.Key,
						OnClick = o => Cmd.Start(path, Log, string.Empty, false)
					});
			}
		}

		private void AppendMsBuildSubMenu()
		{
			var files = GetFiles("*.build").OrderBy(x => x.Key);
			if (!files.Any()) return;
			var item = AddNewMenuItem("MsBuild - *.build", pluginContext.GetIcon(projectContext.MsBuildProvider.PathToMsBuild, 0));
			foreach (var m in files)
			{
				var key = m.Key;
				var path = m.Value;
				item.Items.Add(new UMenuItem
					{
						Header = key,
						OnClick = o => BuildBuildFile(key, path)
					});
			}
		}

		private void BuildBuildFile(string key, string path)
		{
			var r = DialogsCache
				.GetDialog<InputComboBox>()
				.ShowDialog("Please, specify build arguments:",
				            "Build: " + key, projectInfo.MsBuildParams,
				            x => !string.IsNullOrWhiteSpace(x),
				            KnownArgs,
				            null);
			if (r.Key)
			{
				projectInfo.MsBuildParams = r.Value;
				projectContext.MsBuildProvider.BuildBuildFile(path, projectInfo.MsBuildParams);
			}
		}

		private UMenuItem AddNewMenuItem(string header, Icon icon)
		{
			var item = new UMenuItem
				{
					Header = header,
					Icon = icon
				};
			MenuItem.Items.Add(item);
			return item;
		}
	}
}
