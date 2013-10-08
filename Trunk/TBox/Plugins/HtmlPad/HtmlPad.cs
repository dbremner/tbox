using Interface;
using Interface.Atrributes;
using Localization.Plugins.HtmlPad;
using WPFSyntaxHighlighter;

namespace HtmlPad
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
