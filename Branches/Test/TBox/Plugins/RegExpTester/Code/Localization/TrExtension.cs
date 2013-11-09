using System.Resources;
using Localization.Plugins.RegExpTester;

namespace RegExpTester.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
		public TrExtension(string key) : base(key){}

		protected override ResourceManager Manager
		{
            get { return RegExpTesterLang.ResourceManager; }
		}
	}
}
