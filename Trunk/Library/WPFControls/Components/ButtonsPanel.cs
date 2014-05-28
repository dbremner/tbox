using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Mnk.Library.WpfControls.Components
{
	public class ButtonsPanel : UserControl
	{
		private readonly StackPanel spPanel = new StackPanel();
		public ButtonsPanel()
		{
			Content = spPanel;
		}

		public Orientation Orientation
		{
			get { return spPanel.Orientation; }
			set { spPanel.Orientation = value; }
		}

		public void SetButtons(IEnumerable<ButtonsPanelInfo> buttons)
		{
			spPanel.Children.Clear();
			foreach (var info in buttons)
			{
				var btn = new Button
				{
					Content = info.Name,
					Margin = new Thickness(5),
					Width = 75,
				};
				var tmp = info;
				btn.Click += (o, e) => tmp.OnClick();
				spPanel.Children.Add(btn);
			}
		}
	}
}
