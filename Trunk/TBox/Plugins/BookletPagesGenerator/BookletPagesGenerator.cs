using System.Runtime.CompilerServices;
using BookletPagesGenerator.Code;
using Interface;
using Interface.Atrributes;

[assembly: InternalsVisibleTo("UnitTests")]
namespace BookletPagesGenerator
{
	[PluginName("BookletPagesGenerator")]
	[PluginDescription("Page numbers generator, to print books on your printer.")]
	public sealed class BookletPagesGenerator : SingleDialogPlugin<Config, Dialog>
	{
		public BookletPagesGenerator() : base("Print...")
		{
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			Icon = context.GetSystemIcon(16);
		}
	}
}
