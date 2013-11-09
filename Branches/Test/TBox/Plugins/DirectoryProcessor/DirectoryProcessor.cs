using System.IO;
using System.Linq;
using DirectoryProcessor.Code;
using Interface;
using Interface.Atrributes;
using Localization.Plugins.DirectoryProcessor;
using WPFWinForms;

namespace DirectoryProcessor
{
	[PluginInfo(typeof(DirectoryProcessorLang), 4, PluginGroup.Desktop)]
	public sealed class DirectoryProcessor : ConfigurablePlugin<Settings, Config>
	{
		private readonly Executor executor = new Executor();

		public override void OnRebuildMenu()
		{
			base.OnRebuildMenu();
			Menu = Config.Directories
				.Where(x => x.IsChecked)
				.Select(x=>new UMenuItem{Header = Path.GetFileName(x.Key), Items = executor.Process(x)})
				.ToArray();
		}
	}
}
