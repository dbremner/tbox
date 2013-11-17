using System.Resources;
using Localization.Plugins.SqlRunner;

namespace SqlRunner.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, SqlRunnerLang.ResourceManager) { }
	}
}
