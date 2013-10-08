using System.Resources;
using Localization.TBox;

namespace TBox.Code.Localization
{
	public class TranslateExtension : WPFControls.Localization.TranslateExtension
	{
		public TranslateExtension(string key) : base(key)
		{
		}

		protected override ResourceManager Manager
		{
			get { return TBoxLang.ResourceManager; }
		}
	}
}
