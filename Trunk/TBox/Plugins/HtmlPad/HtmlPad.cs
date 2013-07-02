using Interface;
using Interface.Atrributes;
using WPFSyntaxHighlighter;

namespace HtmlPad
{
	[PluginName("HtmlPad")]
	[PluginDescription("Simple plugin to edit HTML in WYSIWYG style")]
	public sealed class HtmlPad : SingleDialogPlugin<Config, Dialog>
	{
		public HtmlPad(): base("Edit...")
		{
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			Icon = context.GetSystemIcon(220);
			context.AddTypeToWarmingUp(typeof(SyntaxHighlighter));
		}
	}
}
