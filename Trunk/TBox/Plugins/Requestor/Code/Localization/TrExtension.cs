using Localization.Plugins.Requestor;

namespace Requestor.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, RequestorLang.ResourceManager) { }
	}
}
