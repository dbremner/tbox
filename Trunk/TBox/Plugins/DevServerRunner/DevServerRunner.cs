using System;
using System.IO;
using System.Linq;
using Common.Tools;
using DevServerRunner.Code.Settings;
using Interface;
using Interface.Atrributes;
using Localization.Plugins.DevServerRunner;
using PluginsShared.Tools;
using WPFWinForms;

namespace DevServerRunner
{
	[PluginInfo(typeof(DevServerRunnerLang), typeof(Properties.Resources), PluginGroup.Web)]
	public sealed class DevServerRunner : ConfigurablePlugin<Settings, Config>
	{
		private CassiniRunner runner = null;

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
							Header = DevServerRunnerLang.RunAll, 
							OnClick = o=>StartAll()
						},
						new UMenuItem{
							IsEnabled = selected.Length>0,
							Header = DevServerRunnerLang.StopAll, 
							OnClick = o=>StopAll()
						},
						new UMenuItem{
							IsEnabled = selected.Length>0,
							Header = DevServerRunnerLang.RestartAll, 
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
