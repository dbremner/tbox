using System.Runtime.CompilerServices;
using BookletPagesGenerator.Code;
using Interface;
using Interface.Atrributes;
using Localization.Plugins.BookletPagesGenerator;

[assembly: InternalsVisibleTo("UnitTests")]
namespace BookletPagesGenerator
{
	[PluginInfo(typeof(BookletPagesGeneratorLang), 16, PluginGroup.Other)]
	public sealed class BookletPagesGenerator : SingleDialogPlugin<Config, Dialog>
	{
		public BookletPagesGenerator() : base(BookletPagesGeneratorLang.Print)
		{
		}
	}
}
