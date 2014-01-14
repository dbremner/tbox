using System.Runtime.CompilerServices;
using Mnk.TBox.Core.Interface;
using Mnk.TBox.Core.Interface.Atrributes;
using Mnk.TBox.Locales.Localization.Plugins.BookletPagesGenerator;
using Mnk.TBox.Plugins.BookletPagesGenerator.Code;

namespace Mnk.TBox.Plugins.BookletPagesGenerator
{
	[PluginInfo(typeof(BookletPagesGeneratorLang), 16, PluginGroup.Other)]
	public sealed class BookletPagesGenerator : SingleDialogPlugin<Config, Dialog>
	{
		public BookletPagesGenerator() : base(BookletPagesGeneratorLang.Print)
		{
		}
	}
}
