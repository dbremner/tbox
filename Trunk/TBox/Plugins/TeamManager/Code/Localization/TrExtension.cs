﻿using Mnk.TBox.Locales.Localization.Plugins.TeamManager;

namespace Mnk.TBox.Plugins.TeamManager.Code.Localization
{
	public class TrExtension : Mnk.Library.WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, TeamManagerLang.ResourceManager) { }
	}
}
