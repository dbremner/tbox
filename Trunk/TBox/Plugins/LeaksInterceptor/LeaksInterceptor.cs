using System;
using Interface;
using Interface.Atrributes;
using LeaksInterceptor.Components;
using Localization.Plugins.LeaksInterceptor;

namespace LeaksInterceptor
{
	[PluginInfo(typeof(LeaksInterceptorLang), 165, PluginGroup.Desktop)]
	public sealed class LeaksInterceptor : SingleDialogConfigurablePlugin<Settings, Config, Dialog>, IDisposable
	{
		public LeaksInterceptor() : base(LeaksInterceptorLang.Analysis)
		{
		}

		protected override Dialog CreateDialog()
		{
			var dialog = base.CreateDialog();
			dialog.Init(Icon);
			return dialog;
		}
	}
}
