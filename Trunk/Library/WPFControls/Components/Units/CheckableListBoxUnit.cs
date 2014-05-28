using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Mnk.Library.WpfControls.Components.Units
{
	public sealed class CheckableListBoxUnit : BaseCheckableCollectionUnit
	{
		protected override Selector CreateItems()
		{
			var cb = new CheckableListBox { Margin = new Thickness(5), TabIndex = 0 };
            cb.SetValue(ScrollViewer.CanContentScrollProperty, CanContentScroll);
		    return cb;

		}

        public bool CanContentScroll { get; set; }
	}
}
