using Localization.Plugins.LeaksInterceptor;

namespace LeaksInterceptor.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, LeaksInterceptorLang.ResourceManager) { }
	}
}
