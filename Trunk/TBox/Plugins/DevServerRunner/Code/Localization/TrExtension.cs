using Localization.Plugins.DevServerRunner;

namespace DevServerRunner.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, DevServerRunnerLang.ResourceManager) { }
	}
}
