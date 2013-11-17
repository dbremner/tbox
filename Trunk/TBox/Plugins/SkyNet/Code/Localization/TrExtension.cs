using Localization.Plugins.SkyNet;
using WPFControls.Localization;

namespace SkyNet.Code.Localization
{
	public class TrExtension : TranslateExtension
	{
        public TrExtension(string key) : base(key, SkyNetLang.ResourceManager) { }
	}
}
