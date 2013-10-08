﻿using System.Resources;
using Localization.Plugins.SourcesUniter;

namespace SourcesUniter.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
		public TrExtension(string key) : base(key){}

		protected override ResourceManager Manager
		{
            get { return SourcesUniterLang.ResourceManager; }
		}
	}
}
