using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Mnk.Library.WpfControls.Components.DropDown
{
	/// <summary>
	/// Interaction logic for BaseDropDownControl.xaml
	/// </summary>
	public partial class BaseDropDownControl
	{
		public BaseDropDownControl()
		{
			InitializeComponent();
			BorderBrush = Brushes.DarkGray;
			BorderThickness = new Thickness(1);
			ValueText.BorderThickness = new Thickness(0);
		}

		public new void Focus()
		{
			ValueText.Focus();
		}

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Up:
					UpButtonClick(null, null);
					break;
				case Key.Down:
					DownButtonClick(null, null);
					break;
				case Key.Tab:
				case Key.Space:
					e.Handled = true;
					break;
			}
			base.OnPreviewKeyDown(e);
		}

		protected virtual void ChangeValue(int nSteps)
		{
			
		}

		protected virtual void TrySaveValue()
		{
			
		}

		private void UpButtonClick(object sender, EventArgs e)
		{
			TrySaveValue();
			ChangeValue(1);
			Focus();
		}

		private void DownButtonClick(object sender, EventArgs e)
		{
			TrySaveValue();
			ChangeValue(-1);
			Focus();
		}

		private void OnMouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (!ValueText.IsFocused) return;
			TrySaveValue();
			ChangeValue(e.Delta / Math.Abs(e.Delta));
		}

		private void OnLostFocus(object sender, RoutedEventArgs e)
		{
			TrySaveValue();
		}
	}
}
