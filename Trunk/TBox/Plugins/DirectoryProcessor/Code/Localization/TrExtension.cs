using System.Resources;
using Localization.Plugins.DirectoryProcessor;

namespace DirectoryProcessor.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, DirectoryProcessorLang.ResourceManager) { }
	}
}
