﻿using System;
using System.Collections;
using System.Linq;
using WPFControls.Code.DataManagers;
using WPFControls.Code.Dialogs;

namespace WPFControls.Code.EditPanel
{
	class Configuration
	{
		private readonly Func<int[]> getSelectedIndexes;
		private readonly Action<int> setSelectedIndexes;
		public readonly IList Items;
		public int[] SelectedIndexes
		{
			get { return getSelectedIndexes(); }
		}
        public int SelectedIndex
        {
            //get { return getSelectedIndexes().FirstOrDefault(); }
            set { setSelectedIndexes(value); }
        }
		public readonly BaseDialog Dialog;
		public readonly IDataManager DataManager;
		public readonly Controller Controller;

		public Configuration(IList items,
			Func<int[]> getSelectedIndexes,
			Action<int> setSelectedIndexes, 
			BaseDialog dialog, 
			IDataManager dataManager, 
			Controller controller)
		{
			Items = items;
			this.getSelectedIndexes = getSelectedIndexes;
			this.setSelectedIndexes = setSelectedIndexes;
			Dialog = dialog;
			DataManager = dataManager;
			Controller = controller;
		}

	}
}
