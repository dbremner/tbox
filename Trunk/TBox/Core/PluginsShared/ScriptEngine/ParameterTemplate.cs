using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Common.Tools;
using Common.UI.Model;
using Localization.PluginsShared;
using ScriptEngine.Core.Params;
using WPFControls.Code.DataManagers;
using WPFControls.Code.Dialogs;
using WPFControls.Components.Units;
using WPFControls.Controls;
using WPFControls.Controls.Captioned;
using WPFControls.Controls.DropDown;
using WPFControls.Tools;

namespace PluginsShared.ScriptEngine
{
	public sealed class ParameterTemplate : UserControl
	{
        private Window Instance { get { return this.GetParentWindow(); } }
		public ParameterTemplate()
		{
			DataContextChanged += OnDataContextChanged;
		}

		private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			var p = DataContext as Parameter;
			Content = (p == null) ? null : CreateChildrenByParameter((dynamic)p);
		}

		private UIElement CreateChildrenByParameter(BoolParameter p)
		{
			var el = new CheckBox();
			el.SetBinding(ToggleButton.IsCheckedProperty, new Binding("Value"));
			return el;
		}

		private UIElement CreateChildrenByParameter(StringParameter p)
		{
			var el = new TextBox ();
			el.SetBinding(TextBox.TextProperty, new Binding("Value"));
			return el;
		}

        private UIElement CreateChildrenByParameter(PasswordParameter p)
        {
            var el = new CaptionedPasswordBox();
            el.SetBinding(CaptionedPasswordBox.ValueProperty, new Binding("Value"));
            return el;
        }

		private UIElement CreateChildrenByParameter(GuidParameter p)
		{
			var el = new TextBox();
			el.SetBinding(TextBox.TextProperty, new Binding("Value"));
			return el;
		}

		private UIElement CreateChildrenByParameter(IntegerParameter p)
		{
			var el = new NumericUpDown { Minimum = p.Min, Maximum = p.Max };
			el.SetBinding(NumericUpDown.ValueProperty, new Binding("Value"));
			return el;
		}

		private UIElement CreateChildrenByParameter(DoubleParameter p)
		{
			var el = new DoubleUpDown { Minimum = p.Min, Maximum = p.Max };
			el.SetBinding(DoubleUpDown.ValueProperty, new Binding("Value"));
			return el;
		}

		private UIElement CreateChildrenByParameter(FileParameter p)
		{
			var el = new EditPath { PathGetterType = PathGetterType.File };
			el.SetBinding(EditPath.ValueProperty, new Binding("Value"));
			return el;
		}

		private UIElement CreateChildrenByParameter(DirectoryParameter p)
		{
			var el = new EditPath { PathGetterType = PathGetterType.Folder };
			el.SetBinding(EditPath.ValueProperty, new Binding("Value"));
			return el;
		}

		private UIElement CreateChildrenByParameter(StringListParameter p)
		{
			var el = new CheckableListBoxUnit { CustomDataManager = new StringValueDataManager<CheckableData<string>>()};
			PrepareListBoxItemTemplate(el, p);
			el.ConfigureInputText(PluginsSharedLang.ConfigureString, p.Values, WPFControls.Code.Dialogs.Templates.Default, x => ParametersValidator.ValidateValue(p, x) && p.Values.All(y => !y.Value.EqualsIgnoreCase(x)), Instance);
			return el;
		}

		private UIElement CreateChildrenByParameter(GuidListParameter p)
		{
			var el = new CheckableListBoxUnit {CustomDataManager = new GuidValueDataManager<CheckableData<Guid>>() };
			PrepareListBoxItemTemplate(el, p);
			Guid tmp;
            el.ConfigureInputText(PluginsSharedLang.ConfigureGuid, p.Values, WPFControls.Code.Dialogs.Templates.Default, x => Guid.TryParse(x, out tmp) && ParametersValidator.ValidateValue(p, tmp) && p.Values.All(y => y.Value != tmp), Instance);
			return el;
		}

		private UIElement CreateChildrenByParameter(IntegerListParameter p)
		{
			var el = new CheckableListBoxUnit { CustomDataManager = new IntValueDataManager<CheckableData<int>>() };
			PrepareListBoxItemTemplate(el, p);
			int tmp;
            el.ConfigureInputText(PluginsSharedLang.ConfigureInt, p.Values, WPFControls.Code.Dialogs.Templates.Default, x => int.TryParse(x, out tmp) && ParametersValidator.ValidateValue(p, tmp) && p.Values.All(y => y.Value != tmp), Instance);
			return el;
		}

		private UIElement CreateChildrenByParameter(DoubleListParameter p)
		{
			var el = new CheckableListBoxUnit { CustomDataManager = new DoubleValueDataManager<CheckableData<double>>() };
			PrepareListBoxItemTemplate(el, p);
			double tmp;
            el.ConfigureInputText(PluginsSharedLang.ConfigureDouble, p.Values, WPFControls.Code.Dialogs.Templates.Default, x => double.TryParse(x, out tmp) && ParametersValidator.ValidateValue(p, tmp) && p.Values.All(y => y.Value != tmp), Instance);
			return el;
		}

		private UIElement CreateChildrenByParameter(FileListParameter p)
		{
			var el = new CheckableListBoxUnit { CustomDataManager = new StringValueDataManager<CheckableData<string>>()};
            PrepareFileListBoxItemTemplate(el, p, PathGetterType.File);
            el.ConfigureInputFilePath(PluginsSharedLang.ConfigureFilePath, p.Values, PathTemplates.Default, x => ParametersValidator.ValidateValue(p, x) && p.Values.All(y => !y.Value.EqualsIgnoreCase(x)), Instance);
			return el;
		}

		private UIElement CreateChildrenByParameter(DirectoryListParameter p)
		{
			var el = new CheckableListBoxUnit { CustomDataManager = new StringValueDataManager<CheckableData<string>>()};
            PrepareFileListBoxItemTemplate(el, p, PathGetterType.Folder);
            el.ConfigureInputFolderPath(PluginsSharedLang.ConfigureDirectoryPath, p.Values, PathTemplates.Default, x => ParametersValidator.ValidateValue(p, x) && p.Values.All(y => !y.Value.EqualsIgnoreCase(x)), Instance);
			return el;
		}

		private static void PrepareListBoxItemTemplate<T>(T el, Parameter p)
            where T : UserControl, ICheckableUnit
		{
			
			var tb = new FrameworkElementFactory(typeof (TextBlock));
            tb.SetBinding(TextBlock.TextProperty, new Binding("Value"));
            PrepareListBoxItemTemplate(el, p, tb);
		}

        private static void PrepareFileListBoxItemTemplate<T>(T el, Parameter p, PathGetterType type)
            where T : UserControl, ICheckableUnit
        {

            var tb = new FrameworkElementFactory(typeof(EditPath));
            tb.SetValue(EditPath.PathGetterTypeProperty, type);
            tb.SetBinding(EditPath.ValueProperty, new Binding("Value"));
            PrepareListBoxItemTemplate(el, p, tb);
        }

        private static void PrepareListBoxItemTemplate<T>(T el, Parameter p, FrameworkElementFactory tb)
            where T : UserControl, ICheckableUnit
        {
            var layout = new DataTemplate { DataType = p.GetType() };
            var factory = new FrameworkElementFactory(typeof(DockPanel));
            var cb = new FrameworkElementFactory(typeof(ExtCheckBox));
            cb.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
            cb.SetValue(PaddingProperty, new Thickness(5, 0, 5, 0));
            cb.SetBinding(ToggleButton.IsCheckedProperty, new Binding("IsChecked"));
            var ev = new RoutedEventHandler(el.OnCheckChangedEvent);
            cb.AddHandler(ToggleButton.CheckedEvent, ev);
            cb.AddHandler(ToggleButton.UncheckedEvent, ev);
            factory.AppendChild(cb);
            tb.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
            tb.SetValue(TextBlock.PaddingProperty, new Thickness(0));
            tb.SetBinding(IsEnabledProperty, new Binding("IsChecked"));
            factory.AppendChild(tb);
            layout.VisualTree = factory;
            el.ItemTemplate = layout;
        }

		private UIElement CreateChildrenByParameter<T>(DictionaryParameter<T> p)
		{
			var el = new CheckableListBoxUnit();
			PrepareDictionaryBoxItemTemplate(el, p);
			el.ConfigureInputText(PluginsSharedLang.ConfigureItem, p.Values, owner: Instance);
			return el;
		}

		private static void PrepareDictionaryBoxItemTemplate(CheckableListBoxUnit el, Parameter p)
		{
			var layout = new DataTemplate { DataType = p.GetType() };
			var factory = new FrameworkElementFactory(typeof(DockPanel));
			var cb = new FrameworkElementFactory(typeof(ExtCheckBox));
			cb.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
			cb.SetValue(PaddingProperty, new Thickness(5, 0, 5, 0));
			cb.SetValue(MarginProperty, new Thickness(2));
			cb.SetBinding(ToggleButton.IsCheckedProperty, new Binding("IsChecked"));
			var ev = new RoutedEventHandler(el.OnCheckChangedEvent);
			cb.AddHandler(ToggleButton.CheckedEvent, ev);
			cb.AddHandler(ToggleButton.UncheckedEvent, ev);
			factory.AppendChild(cb);
			var tbv = CreateFrameworkElementFactory((dynamic)p);
			tbv.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
			tbv.SetValue(PaddingProperty, new Thickness(0));
			tbv.SetValue(DockPanel.DockProperty, Dock.Bottom);
            tbv.SetBinding(IsEnabledProperty, new Binding("IsChecked"));
			factory.AppendChild(tbv);
			var tb = new FrameworkElementFactory(typeof(TextBlock));
			tb.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
			tb.SetValue(TextBlock.PaddingProperty, new Thickness(0));
			tb.SetValue(TextBlock.FontWeightProperty, FontWeights.Heavy);
			tb.SetBinding(TextBlock.TextProperty, new Binding("Key"));
			factory.AppendChild(tb);
			layout.VisualTree = factory;
			el.ItemTemplate = layout;
		}

		private static FrameworkElementFactory CreateFrameworkElementFactory(IStringParameter p)
		{
			var el = new FrameworkElementFactory(typeof (TextBox));
			el.SetBinding(TextBox.TextProperty, new Binding("Value"));
			return el;
		}

		private static FrameworkElementFactory CreateFrameworkElementFactory(IBoolParameter p)
		{
			var el = new FrameworkElementFactory(typeof(CheckBox));
			el.SetBinding(ToggleButton.IsCheckedProperty, new Binding("IsChecked"));
			return el;
		}

		private static FrameworkElementFactory CreateFrameworkElementFactory(IGuidParameter p)
		{
			var el = new FrameworkElementFactory(typeof(TextBox));
			el.SetBinding(TextBox.TextProperty, new Binding("Value"));
			return el;
		}

		private static FrameworkElementFactory CreateFrameworkElementFactory(IFileParameter p)
		{
			var el = new FrameworkElementFactory(typeof(EditPath));
			el.SetBinding(EditPath.ValueProperty, new Binding("Value"));
			el.SetValue(EditPath.PathGetterTypeProperty, PathGetterType.File);
			return el;
		}

		private static FrameworkElementFactory CreateFrameworkElementFactory(IDirectoryParameter p)
		{
			var el = new FrameworkElementFactory(typeof(EditPath));
			el.SetBinding(EditPath.ValueProperty, new Binding("Value"));
			el.SetValue(EditPath.PathGetterTypeProperty, PathGetterType.Folder);
			return el;
		}

		private static FrameworkElementFactory CreateFrameworkElementFactory(IIntegerParameter p)
		{
			var el = new FrameworkElementFactory(typeof(NumericUpDown));
			el.SetValue(NumericUpDown.MinimumProperty, p.Min);
			el.SetValue(NumericUpDown.MaximumProperty, p.Max);
			el.SetBinding(NumericUpDown.ValueProperty, new Binding("Value"));
			return el;
		}

		private static FrameworkElementFactory CreateFrameworkElementFactory(IDoubleParameter p)
		{
			var el = new FrameworkElementFactory(typeof(DoubleUpDown));
			el.SetValue(DoubleUpDown.MinimumProperty, p.Min);
			el.SetValue(DoubleUpDown.MaximumProperty, p.Max);
			el.SetBinding(DoubleUpDown.ValueProperty, new Binding("Value"));
			return el;
		}
	}
}
