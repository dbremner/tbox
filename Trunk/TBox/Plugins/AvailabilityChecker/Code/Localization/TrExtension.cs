﻿using Localization.Plugins.AvailabilityChecker;

namespace AvailabilityChecker.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, AvailabilityCheckerLang.ResourceManager) { }
	}
}
