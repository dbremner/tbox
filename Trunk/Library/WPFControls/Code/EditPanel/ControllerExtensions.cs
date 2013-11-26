using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Common.UI.Model;
using WPFControls.Code.DataManagers;
using WPFControls.Code.Dialogs;
using WPFControls.Components.EditButtons;
using WPFControls.Tools;

namespace WPFControls.Code.EditPanel
{
	public static class ControllerExtensions
	{
		public static void ConfigureSelector<T>(this Selector selector,
			Collection<T> collection, EditButtonsPanel editButtonsPanel, BaseDialog dialog, IDataManager dataManager)
			where T : Data, ICloneable, new()
		{
			var controller = new Controller(selector.Items.Refresh);
			controller.Config = new Configuration(collection,
                    () => (selector is ListBox) ? ((ListBox)selector).GetSelectedIds().ToArray() : new[] { selector.SelectedIndex },
					(selector is ComboBox) ? SelectCb(selector) : Select(selector),
					dialog,
					dataManager,
					controller);
			selector.ItemsSource = collection;
			selector.SelectionChanged +=
				(o, args) => editButtonsPanel.OnSelectionChanged();
			editButtonsPanel.SetConfig(controller.Config);
		}

	    public static void ConfigureSelector<T>(this Selector selector,
			Collection<T> collection, EditButtonsPanel editButtonsPanel, BaseDialog dialog)
			where T : Data, ICloneable, new()
		{
			selector.ConfigureSelector(collection, editButtonsPanel, dialog, new DataItemManager<T>());
		}

		private static Action<int> Select(Selector cb)
		{
			return id => cb.SelectedIndex = id;
		}

		private static Action<int> SelectCb(Selector cb)
		{
			return id =>
			    {
			        var i = id;
				if (cb.SelectedIndex == i)
					cb.SelectedIndex = -1;
				cb.SelectedIndex = i;
			};
		}
	}
}