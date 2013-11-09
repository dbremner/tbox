using System.Windows;
using System.Windows.Controls.Primitives;
using WPFControls.Controls;

namespace WPFControls.Components.Units
{
	public sealed class CheckableFileListBoxUnit: BaseCheckableCollectionUnit
	{
		protected override Selector CreateItems()
		{
			return new CheckableFileListBox { Margin = new Thickness(5), TabIndex = 0 };
		}

		public readonly static DependencyProperty PathGetterTypeProperty =
			DpHelper.Create<CheckableFileListBoxUnit, PathGetterType>("PathGetterType",
		(box, type) => box.PathGetterType = type);
		public PathGetterType PathGetterType
		{
			get { return ((CheckableFileListBox)Items).PathGetterType; }
			set { ((CheckableFileListBox)Items).PathGetterType = value; }
		}
	}
}
