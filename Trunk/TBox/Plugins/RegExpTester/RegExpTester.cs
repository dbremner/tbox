using Interface;
using Interface.Atrributes;
using WPFSyntaxHighlighter;

namespace RegExpTester
{
	[PluginName("RegExp tester")]
	[PluginDescription("Simple plugin to build regular expressions and test it.")]
	public sealed class RegExpTester : SingleDialogPlugin<Config, Dialog>
	{
		public RegExpTester():base("Test...")
		{
			Icon = Properties.Resources.Icon;
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			context.AddTypeToWarmingUp(typeof(SyntaxHighlighter));
		}
	}
}
