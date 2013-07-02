using Interface;
using Interface.Atrributes;

namespace SourcesUniter
{
	[PluginName("SourcesUniter")]
	[PluginDescription("Simple plugin to unite big set of sources in to the one document.\r\nDesigned mostly for studients.")]
	public sealed class SourcesUniter : SingleDialogPlugin<Config, Dialog>
	{
		public SourcesUniter() : base("Unite...")
		{
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			Icon = Context.GetSystemIcon(68);
		}
	}
}
