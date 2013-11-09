﻿using System.Resources;
using Localization.Plugins.Requestor;

namespace Requestor.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
		public TrExtension(string key) : base(key){}

		protected override ResourceManager Manager
		{
            get { return RequestorLang.ResourceManager; }
		}
	}
}