using System.Resources;
using Localization.Plugins.NUnitRunner;

namespace NUnitRunner.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, NUnitRunnerLang.ResourceManager) { }
	}
}
