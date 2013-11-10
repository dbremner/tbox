using System.Resources;
using Localization.TBox;
using WPFControls.Localization;

namespace TBox.Code.Localization
{
	public class TrExtension : TranslateExtension
	{
        public TrExtension(string key)
            : base(key)
		{
		}

		protected override ResourceManager Manager
		{
			get { return TBoxLang.ResourceManager; }
		}
	}
}
