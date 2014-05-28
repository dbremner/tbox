﻿using System;
using System.IO;
using System.Linq;
using System.Windows;
using Mnk.Library.Common.MT;
using Mnk.Library.Common.Tools;
using Mnk.TBox.Core.Contracts;
using Mnk.TBox.Locales.Localization.Plugins.DevServerRunner;
using Mnk.TBox.Core.PluginsShared.Tools;
using Mnk.Library.WpfControls.Dialogs;
using Mnk.Library.WpfWinForms;
using Mnk.Library.WpfWinForms.Icons;
using Mnk.TBox.Plugins.DevServerRunner.Code.Settings;

namespace Mnk.TBox.Plugins.DevServerRunner
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
            DialogsCache.ShowProgress(
                StartAll, 
                DevServerRunnerLang.RunAll, null, icon: Icon.ToImageSource(), showInTaskBar:true );
		}

	    private void StartAll(IUpdater u)
	    {
	        var i = 0;
	        var items = Config.ServerInfos.CheckedItems.ToArray();
	        foreach (var info in items)
	        {
	            ProcessFolder(info);
                u.Update(info.Key, ++i/(float)items.Length);
	        }
	    }

	    private void ProcessFolder(ServerInfo info)
		{
			runner.Run(
				info.Key, info.Port, info.VPath, info.AdminPrivilegies, 
				Config.PathToDevServer, Config.PathToBrowser, Config.RunBrowser);
		}
	}
}
