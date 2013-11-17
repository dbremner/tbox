using System;
using WPFControls.Code;
using WPFControls.Dialogs;
using WPFControls.Dialogs.StateSaver;
using WPFWinForms;
using WPFWinForms.Icons;

namespace Interface
{
	public class SingleDialogPlugin<TConfig, TDialog> : SimpleConfigurablePlugin<TConfig>, IDisposable
		where TConfig : IConfigWithDialogStates, new()
		where TDialog : DialogWindow, new()
	{
		protected readonly LazyDialog<TDialog> Dialog;
		protected SingleDialogPlugin(string menuItemName)
		{
			Menu = new[] { new UMenuItem { Header = menuItemName, OnClick = o=>ShowDialog() } };
			Dialog = new LazyDialog<TDialog>(CreateDialog, "default");
		}

		protected virtual TDialog CreateDialog()
		{
			return new TDialog
			{
                DataContext = ConfigManager.Config, 
				Icon = Icon.ToImageSource()
			};
		}

		protected virtual void ShowDialog()
		{
            Dialog.Show(Context.DoSync, ConfigManager.Config.States);
		}

		protected override void OnConfigUpdated()
		{
			base.OnConfigUpdated();
			if (!Dialog.IsValueCreated)return;
            Dialog.Value.DataContext = ConfigManager.Config;
			Dialog.Hide();
		}

		public override void Save(bool autoSaveOnExit)
		{
			base.Save(autoSaveOnExit);
            if (autoSaveOnExit) Dialog.SaveState(ConfigManager.Config.States);
		}

		public virtual void Dispose()
		{
			Dialog.Dispose();
		}
	}
}
