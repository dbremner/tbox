using Localization.Plugins.TextGenerator;

namespace TextGenerator.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, TextGeneratorLang.ResourceManager) { }
	}
}
