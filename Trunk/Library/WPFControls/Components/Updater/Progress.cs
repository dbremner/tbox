using System;
using System.Windows;
using System.Windows.Controls;
using Common.MT;
using WPFControls.Code.OS;

namespace WPFControls.Components.Updater
{
	public class Progress : SimpleProgress
	{
		private readonly Label lMessage = new Label{Padding = new Thickness(5,5,5,0), Height = 20};
		public Progress()
		{
			var panel = (DockPanel)Content;
			panel.Children.Insert(0, lMessage);
			DockPanel.SetDock(lMessage, Dock.Top);
		}

		public override void Reset()
		{
			base.Reset();
			lMessage.Content = string.Empty;
		}

		public void SetMessage(string value)
		{
			Mt.SetText(lMessage, value);
		}

		protected override IUpdater CreateUpdater(Action action)
		{
			return new Updater(this, action);
		}
	}
}
