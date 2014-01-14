﻿using Mnk.TBox.Core.Interface;
using Mnk.TBox.Core.Interface.Atrributes;
using Mnk.TBox.Locales.Localization.Plugins.RegExpTester;
using Mnk.Library.WPFSyntaxHighlighter;

namespace Mnk.TBox.Plugins.RegExpTester
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
