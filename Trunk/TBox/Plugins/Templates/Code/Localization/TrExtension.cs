﻿using Mnk.TBox.Locales.Localization.Plugins.Templates;

namespace Mnk.TBox.Plugins.Templates.Code.Localization
{
	public class TrExtension : Mnk.Library.WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, TemplatesLang.ResourceManager) { }
	}
}
