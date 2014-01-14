using Mnk.TBox.Locales.Localization.TBox;
using Mnk.Library.WPFControls.Localization;

namespace Mnk.TBox.Core.Application.Code.Localization
{
	public class TrExtension : TranslateExtension
	{
        public TrExtension(string key): base(key, TBoxLang.ResourceManager){}
	}
}
