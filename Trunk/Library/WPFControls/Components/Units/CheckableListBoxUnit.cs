using System.Windows;
using System.Windows.Controls.Primitives;

namespace WPFControls.Components.Units
{
	public sealed class CheckableListBoxUnit : BaseCheckableCollectionUnit
	{
		protected override Selector CreateItems()
		{
			return new CheckableListBox { Margin = new Thickness(5), TabIndex = 0 };
		}
	}
}
