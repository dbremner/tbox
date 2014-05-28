using System.Windows;
using System.Windows.Controls.Primitives;

namespace Mnk.Library.WpfControls.Components.Units
{
	public sealed class ListBoxUnit : BaseCollectionUnit
	{
		public ListBoxUnit()
		{
			Init();
		}

		protected override Selector CreateItems()
		{
			return new ExtListBox { Margin = new Thickness(5), TabIndex = 0 };
		}
	}
}
