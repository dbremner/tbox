using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.WPFControls.Components.FilesAndFolders;

namespace Mnk.Library.WPFControls.Components
{
	public class CheckableFileListBox : CheckableListBox
	{
		public CheckableFileListBox()
		{
			var template = new DataTemplate { DataType = typeof(CheckableData) };
			var panel = new FrameworkElementFactory(typeof(DockPanel));
			panel.SetValue(PaddingProperty, new Thickness(2));
			var chb = new FrameworkElementFactory(typeof(ExtCheckBox));
			chb.SetBinding(ToggleButton.IsCheckedProperty, new Binding("IsChecked"));
			chb.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
			chb.SetValue(PaddingProperty, new Thickness(5, 0, 5, 0));
			chb.AddHandler(ExtCheckBox.ValueChangedEvent, new RoutedEventHandler(OnCheckChangedEvent));
			panel.AppendChild(chb);
			var pg = new FrameworkElementFactory(typeof(EditPath));
            pg.SetBinding(IsEnabledProperty, new Binding("IsChecked"));
            pg.SetBinding(EditPath.ValueProperty, new Binding("Key"));
			pg.SetBinding(EditPath.PathGetterTypeProperty, new Binding { Source = this, Path = new PropertyPath("PathGetterType") });
			pg.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
			pg.SetValue(PaddingProperty, new Thickness(0));
			panel.AppendChild(pg);
			template.VisualTree = panel;
			ItemTemplate = template;
		}

		public readonly static DependencyProperty PathGetterTypeProperty =
			DpHelper.Create<CheckableFileListBox, PathGetterType>("PathGetterType", 
				(box, type) => box.PathGetterType = type);
		public PathGetterType PathGetterType
		{
			get { return (PathGetterType)GetValue(PathGetterTypeProperty); }
			set { SetValue(PathGetterTypeProperty, value); }
		}
	}
}
