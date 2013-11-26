using Localization.TBox;
using WPFControls.Localization;

namespace TBox.Code.Localization
{
	public class TrExtension : TranslateExtension
	{
        public TrExtension(string key): base(key, TBoxLang.ResourceManager){}
	}
}
