﻿using Mnk.TBox.Locales.Localization.Plugins.ServicesCommander;

namespace Mnk.TBox.Plugins.ServicesCommander.Code.Localization
{
	public class TrExtension : Mnk.Library.WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, ServicesCommanderLang.ResourceManager) { }
	}
}
