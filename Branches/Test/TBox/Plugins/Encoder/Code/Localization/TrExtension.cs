using System.Resources;
using Localization.Plugins.Encoder;

namespace Encoder.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
		public TrExtension(string key) : base(key){}

		protected override ResourceManager Manager
		{
			get { return EncoderLang.ResourceManager; }
		}
	}
}
