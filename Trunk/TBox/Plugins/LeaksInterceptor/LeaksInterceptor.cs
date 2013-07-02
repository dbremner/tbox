using System;
using Interface;
using Interface.Atrributes;
using LeaksInterceptor.Components;

namespace LeaksInterceptor
{
	[PluginName("Leaks Interceptor")]
	[PluginDescription("Created for the analysis of the applications and find any leaks.")]
	public sealed class LeaksInterceptor : SingleDialogConfigurablePlugin<Settings, Config, Dialog>, IDisposable
	{
		public LeaksInterceptor() : base("Analysis...")
		{
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			Icon = context.GetSystemIcon(165);
		}

		protected override Dialog CreateDialog()
		{
			var dialog = base.CreateDialog();
			dialog.Init(Icon);
			return dialog;
		}
	}
}
