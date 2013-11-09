using Interface;
using Interface.Atrributes;
using Localization.Plugins.SourcesUniter;
using SourcesUniter.Code;

namespace SourcesUniter
{
	[PluginInfo(typeof(SourcesUniterLang), 68, PluginGroup.Other)]
	public sealed class SourcesUniter : SingleDialogPlugin<Config, Dialog>
	{
		public SourcesUniter() : base(SourcesUniterLang.Unite)
		{
		}
	}
}
