using System;
using System.IO;
using System.Linq;
using Common.Tools;
using DevServerRunner.Code.Settings;
using Interface;
using Interface.Atrributes;
using PluginsShared.Tools;
using WPFWinForms;

namespace DevServerRunner
{
	[PluginName("DevServer runner")]
	[PluginDescription("Small tool to run standard developer server without visual studio.")]
	public sealed class DevServerRunner : ConfigurablePlugin<Settings, Config>
	{
		private CassiniRunner runner = null;

		public DevServerRunner()
		{
			Icon = Properties.Resources.Icon;
		}

		public override void OnRebuildMenu()
		{
			base.OnRebuildMenu();
			if(runner==null)
				runner = new CassiniRunner(Context.DataProvider.ToolsPath);
			var selected = Config.ServerInfos.CheckedItems.ToArray();
			Menu = selected.Select(
				x=>new UMenuItem
					   {
						   Header = Path.GetFileName(x.Key),
						   OnClick = o => ProcessFolder(x)
					   }
				).Concat(
				new[]
					{
						new USeparator(), 
						new UMenuItem{
							IsEnabled = selected.Length>0,
							Header = "Run All", 
							OnClick = o=>StartAll()
						},
						new UMenuItem{
							IsEnabled = selected.Length>0,
							Header = "Stop All", 
							OnClick = o=>StopAll()
						},
						new UMenuItem{
							IsEnabled = selected.Length>0,
							Header = "Restart All", 
							OnClick = o=>RestartAll()
						}
					}
				).ToArray();
		}

		private void RestartAll()
		{
			StopAll();
			StartAll();
		}

		private void StopAll()
		{
			runner.StopAll(Config.PathToDevServer);
		}

		private void StartAll()
		{
			Config.ServerInfos.CheckedItems.ForEach(ProcessFolder);
		}

		private void ProcessFolder(ServerInfo info)
		{
			runner.Run(
				info.Key, info.Port, info.VPath, info.AdminPrivilegies, 
				Config.PathToDevServer, Config.PathToBrowser, Config.RunBrowser);
		}
	}
}
