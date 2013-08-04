﻿using System;
using System.Linq;

namespace WPFControls.Code.EditPanel
{
	public sealed class Controller
	{
		internal Configuration Config { get; set; }

		private readonly Action refreshHandler;
		public Controller(Action refreshHandler)
		{
			this.refreshHandler = refreshHandler;
		}
		
		public bool Add()
		{
			string[] names;
			if (!Config.Dialog.Add(out names)) return false;
			foreach (var name in names)
			{
				Config.Items.Add(Config.DataManager.Create(name));
			}
			Config.SelectedIndex = Config.Items.Count - 1;
			return true;
		}

		public bool Clone()
		{
			string newName;
			var item = Config.Items[Config.SelectedIndexes.First()];
			if (!Config.Dialog.Clone(Config.DataManager.GetKey(item), out newName))
				return false;
			Config.Items.Add(Config.DataManager.Clone(item, newName));
			Config.SelectedIndex = Config.Items.Count-1;
			return true;
		}

		public bool Edit()
		{
			string newName;
			var id = Config.SelectedIndexes.First();
			var item = Config.Items[id];
			if (!Config.Dialog.Edit(Config.DataManager.GetKey(item), out newName))
				return false;
			Config.DataManager.ChangeKey(item, newName);
			refreshHandler();
			Config.SelectedIndex = id;
			return true;
		}

		public bool Del()
		{
			if (!Config.Dialog.Del(
					Config.SelectedIndexes
                    .Select(
                        x=>Config.DataManager.GetKey(Config.Items[x])).ToArray() ))
				return false;
			var ids = Config.SelectedIndexes;
		    var items = ids.Select(x => Config.Items[x]).ToArray();
		    foreach (var item in items)
		    {
                Config.Items.Remove(item);
		    }
			if (Config.Items.Count > 0)
			{
				Config.SelectedIndex = Math.Min(ids.Last(), Config.Items.Count-1);
			}
			return true;
		}

		public bool Clear()
		{
			if (!Config.Dialog.Clear())
				return false;
			Config.Items.Clear();
			return true;
		}
	}
}
