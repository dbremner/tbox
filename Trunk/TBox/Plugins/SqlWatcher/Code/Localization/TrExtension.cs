﻿using Mnk.TBox.Locales.Localization.Plugins.SqlWatcher;

namespace Mnk.TBox.Plugins.SqlWatcher.Code.Localization
{
	public class TrExtension : Mnk.Library.WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, SqlWatcherLang.ResourceManager) { }
	}
}
