using System.IO;
using System.Linq;
using DirectoryProcessor.Code;
using Interface;
using Interface.Atrributes;
using WPFWinForms;

namespace DirectoryProcessor
{
	[PluginName("Directory processor")]
	[PluginDescription("Ability to run any software for any subdirectory of the direcory.\nFor example you can run localization tool by one click.")]
	public sealed class DirectoryProcessor : ConfigurablePlugin<Settings, Config>
	{
		private readonly Executor executor = new Executor();

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			Icon = Context.GetSystemIcon(4);
		}

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
