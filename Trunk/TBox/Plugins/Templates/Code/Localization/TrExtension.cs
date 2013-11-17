using Localization.Plugins.Templates;

namespace Templates.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, TemplatesLang.ResourceManager) { }
	}
}
