﻿using Mnk.TBox.Locales.Localization.Plugins.SourcesUniter;

namespace Mnk.TBox.Plugins.SourcesUniter.Code.Localization
{
	public class TrExtension : Mnk.Library.WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, SourcesUniterLang.ResourceManager) { }
	}
}
