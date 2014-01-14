using System.Resources;
using Mnk.TBox.Locales.Localization.Plugins.NUnitRunner;

namespace Mnk.TBox.Plugins.NUnitRunner.Code.Localization
{
	public class TrExtension : Mnk.Library.WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, NUnitRunnerLang.ResourceManager) { }
	}
}
