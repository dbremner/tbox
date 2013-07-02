using System;
using System.Collections;
using WPFControls.Code.DataManagers;
using WPFControls.Code.Dialogs;

namespace WPFControls.Code.EditPanel
{
	class Configuration
	{
		private readonly Func<int> getSelectedIndex;
		private readonly Action<int> setSelectedIndex;
		public readonly IList Items;
		public int SelectedIndex
		{
			get { return getSelectedIndex(); }
			set { setSelectedIndex(value); }
		}
		public readonly BaseDialog Dialog;
		public readonly IDataManager DataManager;
		public readonly Controller Controller;

		public Configuration(IList items,
			Func<int> getSelectedIndex,
			Action<int> setSelectedIndex, 
			BaseDialog dialog, 
			IDataManager dataManager, 
			Controller controller)
		{
			Items = items;
			this.getSelectedIndex = getSelectedIndex;
			this.setSelectedIndex = setSelectedIndex;
			Dialog = dialog;
			DataManager = dataManager;
			Controller = controller;
		}

	}
}
