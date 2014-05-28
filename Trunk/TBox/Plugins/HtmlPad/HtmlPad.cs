using Mnk.TBox.Core.Interface;
using Mnk.TBox.Core.Interface.Atrributes;
using Mnk.TBox.Locales.Localization.Plugins.HtmlPad;
using Mnk.Library.WpfSyntaxHighlighter;

namespace Mnk.TBox.Plugins.HtmlPad
{
	[PluginInfo(typeof(HtmlPadLang), 220, PluginGroup.Web)]
	public sealed class HtmlPad : SingleDialogPlugin<Config, Dialog>
	{
		public HtmlPad(): base(HtmlPadLang.Edit)
		{
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			context.AddTypeToWarmingUp(typeof(SyntaxHighlighter));
		}
	}
}
