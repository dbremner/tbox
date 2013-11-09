using System.Resources;
using Localization.Plugins.Automator;

namespace Automator.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
		public TrExtension(string key) : base(key){}

		protected override ResourceManager Manager
		{
			get { return AutomatorLang.ResourceManager; }
		}
	}
}
