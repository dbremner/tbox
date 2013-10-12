using System;
using WPFControls.Code;
using WPFControls.Dialogs;
using WPFControls.Dialogs.StateSaver;
using WPFControls.Tools;
using WPFWinForms;

namespace Interface
{
	public abstract class SingleDialogConfigurablePlugin<TSettings, TConfig, TDialog> : ConfigurablePlugin<TSettings, TConfig>, IDisposable
		where TSettings: ISettings, new()
		where TConfig: IConfigWithDialogStates, new()
		where TDialog : DialogWindow, new()
	{
		protected readonly LazyDialog<TDialog> Dialog;
		protected SingleDialogConfigurablePlugin(string menuItemName)
		{
			Menu = new[] { new UMenuItem { Header = menuItemName, OnClick = o => ShowDialog() } };
			Dialog = new LazyDialog<TDialog>(CreateDialog, "default");
		}

		protected virtual TDialog CreateDialog()
		{
            var dialog = new TDialog { DataContext = ConfigManager.Config };
			dialog.Icon = ImageSource;
			return dialog;
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
