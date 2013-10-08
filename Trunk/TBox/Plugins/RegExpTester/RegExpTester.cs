using Interface;
using Interface.Atrributes;
using Localization.Plugins.RegExpTester;
using WPFSyntaxHighlighter;

namespace RegExpTester
{
	[PluginInfo(typeof(RegExpTesterLang), typeof(Properties.Resources), PluginGroup.Development)]
	public sealed class RegExpTester : SingleDialogPlugin<Config, Dialog>
	{
		public RegExpTester():base( RegExpTesterLang.Test)
		{
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			context.AddTypeToWarmingUp(typeof(SyntaxHighlighter));
		}
	}
}
