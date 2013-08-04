using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WPFControls.Controls
{
	public class EditPath : UserControl
	{
		private static readonly IList<string> KnownPathes = new ObservableCollection<string>();
		private readonly DockPanel panel = new DockPanel();
        private readonly AutoComboBox child = new AutoComboBox
            {
                Margin = new Thickness(5, 0, 5, 0), 
                IsEditable = true, 
                AutoSort = true
            };
		private readonly Button btn = new Button { Margin = new Thickness(5, 0, 5, 0), Content = "...", Width = 32};

		public EditPath()
		{
			child.ItemsSource = KnownPathes;
			child.AddHandler(LostKeyboardFocusEvent, new RoutedEventHandler((o, e) =>
				{
                    if (string.Equals(child.Text, GetValue(ValueProperty)))return;
                    OnValueChanged(o, e);
					SetValue(ValueProperty, child.Text);
				}));
			btn.Click += OnBtnClick;
			panel.Children.Add(btn);
			DockPanel.SetDock(btn, Dock.Right);
			panel.Children.Add(child);
			Content = panel;
		}

		public static readonly DependencyProperty ValueProperty =
			DpHelper.Create<EditPath, string>("Value", (s, v) => s.Value = v);
		public string Value
		{
			get { return child.Text; }
			set
			{
				SetValue(ValueProperty, value);
				lock (KnownPathes)
				{
					child.AddKnownValueIfNeed(KnownPathes, value);
				}
				child.Text = value;
			}
		}

		public static readonly DependencyProperty PathGetterTypeProperty =
			DpHelper.Create<EditPath, PathGetterType>("PathGetterType", 
			(s, v) => s.PathGetterType = v);
		public PathGetterType PathGetterType
		{
			get { return (PathGetter is FilePathGetter) ? PathGetterType.File : PathGetterType.Folder; }
			set
			{
				SetValue(PathGetterTypeProperty, value);
				switch (value)
				{
					case PathGetterType.File:
						PathGetter = new FilePathGetter();
						break;
					case PathGetterType.Folder:
						PathGetter = new FolderPathGetter();
						break;
				}
			}
		}

		public new void Focus()
		{
			child.Focus();
		}

		public IPathGetter PathGetter { get; set; }

		private void OnBtnClick(object sender, RoutedEventArgs e)
		{
			var path = Value;
			if (PathGetter.Get(ref path))
			{
				Value = path;
                OnValueChanged(sender, e);
			}
		}

		public event RoutedEventHandler ValueChanged;
		protected void OnValueChanged(object sender, RoutedEventArgs e)
		{
			if (ValueChanged != null) ValueChanged(sender, e);
		}
	}
}
