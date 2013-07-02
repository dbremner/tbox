using System.Windows;
using System.Windows.Controls.Primitives;

namespace WPFControls.Components.Units
{
	public sealed class ListBoxUnit : BaseCollectionUnit
	{
		protected override Selector CreateItems()
		{
			return new ExtListBox { Margin = new Thickness(5), TabIndex = 0 };
		}
	}
}
